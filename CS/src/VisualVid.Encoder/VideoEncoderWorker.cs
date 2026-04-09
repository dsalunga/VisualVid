using System.Data.Common;
using System.Diagnostics;
using System.Net.Mail;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Npgsql;

namespace VisualVid.Encoder;

public class VideoEncoderWorker : BackgroundService
{
    private readonly ILogger<VideoEncoderWorker> _logger;
    private readonly EncoderSettings _settings;
    private bool IsPostgres => _settings.DatabaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase);

    public VideoEncoderWorker(ILogger<VideoEncoderWorker> logger, IOptions<EncoderSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    private DbConnection CreateConnection()
    {
        if (IsPostgres)
            return new NpgsqlConnection(_settings.ConnectionString);
        return new SqlConnection(_settings.ConnectionString);
    }

    private DbCommand CreateCommand(string sql, DbConnection connection)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        return cmd;
    }

    private void AddParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Video Encoder Worker started");

        // Ensure dead-letter directory exists
        if (!string.IsNullOrEmpty(_settings.DeadLetterPath))
            Directory.CreateDirectory(_settings.DeadLetterPath);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Check if ffmpeg is already running
            var ffmpegProcesses = Process.GetProcessesByName("ffmpeg");
            if (ffmpegProcesses.Length > 0)
            {
                _logger.LogInformation("ffmpeg is running. Waiting {Interval}s...", _settings.PollIntervalSeconds);
                await Task.Delay(TimeSpan.FromSeconds(_settings.PollIntervalSeconds), stoppingToken);
                continue;
            }

            bool hasRows = false;

            await using var connection = CreateConnection();
            await connection.OpenAsync(stoppingToken);

            // Use plain SQL instead of stored procedure for cross-DB compatibility
            var pendingSql = IsPostgres
                ? "SELECT v.\"VideoId\", v.\"OriginalExtension\", v.\"UserId\", u.\"Email\", v.\"Title\" FROM \"Videos\" v JOIN \"aspnet_users\" u ON v.\"UserId\" = u.\"user_id\" WHERE v.\"Pending\" = true AND v.\"IsActive\" = false"
                : "SELECT v.VideoId, v.OriginalExtension, v.UserId, u.Email, v.Title FROM Videos v JOIN aspnet_Users u ON v.UserId = u.UserId WHERE v.Pending = 1 AND v.IsActive = 0";

            await using var cmd = CreateCommand(pendingSql, connection);
            await using var reader = await cmd.ExecuteReaderAsync(stoppingToken);

            while (await reader.ReadAsync(stoppingToken))
            {
                hasRows = true;

                var videoId = reader["VideoId"].ToString()!;
                var ext = reader["OriginalExtension"].ToString()!;
                var userId = reader["UserId"].ToString()!;
                var email = reader["Email"].ToString()!;
                var title = reader["Title"].ToString()!;

                var pendingPath = Path.Combine(_settings.PendingPath, $"{videoId}{ext}");
                var memberDir = Path.Combine(_settings.VideoStoragePath, userId);
                var outputPath = Path.Combine(memberDir, $"{videoId}.{_settings.OutputFormat}");
                var thumbnailPath = Path.Combine(memberDir, $"{videoId}.jpg");

                // Idempotency: skip if output already exists and DB is already active
                if (File.Exists(outputPath) && await IsVideoActiveAsync(videoId, stoppingToken))
                {
                    _logger.LogInformation("Video {VideoId} already processed (idempotency check). Skipping.", videoId);
                    CleanupPendingFile(pendingPath);
                    continue;
                }

                Directory.CreateDirectory(memberDir);

                _logger.LogInformation("Processing video {VideoId} ({Title})", videoId, title);

                var success = false;
                for (int attempt = 1; attempt <= _settings.MaxRetries; attempt++)
                {
                    try
                    {
                        // Generate thumbnail
                        await RunProcessAsync(_settings.FfmpegPath,
                            $"-i \"{pendingPath}\" -ss {_settings.ThumbnailTimeOffset} -vframes 1 -y \"{thumbnailPath}\"",
                            stoppingToken);

                        // Encode video
                        await RunProcessAsync(_settings.FfmpegPath,
                            $"-i \"{pendingPath}\" -c:v libx264 -preset medium -crf 23 -c:a aac -b:a 128k -movflags +faststart -y \"{outputPath}\"",
                            stoppingToken);

                        // Update DB: mark as active
                        await using var updateConn = CreateConnection();
                        await updateConn.OpenAsync(stoppingToken);
                        var updateSql = IsPostgres
                            ? "UPDATE \"Videos\" SET \"Pending\" = false, \"IsActive\" = true WHERE \"VideoId\" = @VideoId AND \"Pending\" = true"
                            : "UPDATE Videos SET Pending=0, IsActive=1 WHERE VideoId=@VideoId AND Pending=1";
                        await using var updateCmd = CreateCommand(updateSql, updateConn);
                        AddParameter(updateCmd, "@VideoId", new Guid(videoId));
                        await updateCmd.ExecuteNonQueryAsync(stoppingToken);

                        // Delete pending file
                        CleanupPendingFile(pendingPath);

                        // Send success email
                        await SendEmailSafeAsync(email, title, videoId, success: true);

                        _logger.LogInformation("Video {VideoId} encoded successfully on attempt {Attempt}", videoId, attempt);
                        success = true;
                        break;
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogWarning("Encoding of video {VideoId} cancelled due to shutdown", videoId);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Attempt {Attempt}/{MaxRetries} failed for video {VideoId}",
                            attempt, _settings.MaxRetries, videoId);

                        if (attempt < _settings.MaxRetries)
                        {
                            var delay = _settings.RetryDelaySeconds * attempt; // Linear backoff
                            _logger.LogInformation("Retrying video {VideoId} in {Delay}s...", videoId, delay);
                            await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
                        }
                    }
                }

                if (!success)
                {
                    _logger.LogError("Video {VideoId} failed after {MaxRetries} attempts. Moving to dead-letter.",
                        videoId, _settings.MaxRetries);

                    // Move to dead-letter directory
                    MoveToDeadLetter(pendingPath, videoId, ext);

                    // Mark as failed in DB (set Pending=0, IsActive=0)
                    await MarkAsFailedAsync(videoId, stoppingToken);

                    // Send failure email
                    await SendEmailSafeAsync(email, title, videoId, success: false);
                }
            }

            if (!hasRows)
            {
                _logger.LogDebug("No pending videos. Sleeping {Interval}s...", _settings.PollIntervalSeconds);
            }

            await Task.Delay(TimeSpan.FromSeconds(_settings.PollIntervalSeconds), stoppingToken);
        }
    }

    private async Task<bool> IsVideoActiveAsync(string videoId, CancellationToken ct)
    {
        await using var conn = CreateConnection();
        await conn.OpenAsync(ct);
        var sql = IsPostgres
            ? "SELECT \"IsActive\" FROM \"Videos\" WHERE \"VideoId\" = @VideoId"
            : "SELECT IsActive FROM Videos WHERE VideoId=@VideoId";
        await using var cmd = CreateCommand(sql, conn);
        AddParameter(cmd, "@VideoId", new Guid(videoId));
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is true or 1;
    }

    private async Task MarkAsFailedAsync(string videoId, CancellationToken ct)
    {
        try
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync(ct);
            var sql = IsPostgres
                ? "UPDATE \"Videos\" SET \"Pending\" = false, \"IsActive\" = false WHERE \"VideoId\" = @VideoId"
                : "UPDATE Videos SET Pending=0, IsActive=0 WHERE VideoId=@VideoId";
            await using var cmd = CreateCommand(sql, conn);
            AddParameter(cmd, "@VideoId", new Guid(videoId));
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark video {VideoId} as failed in DB", videoId);
        }
    }

    private void MoveToDeadLetter(string pendingPath, string videoId, string ext)
    {
        if (!File.Exists(pendingPath))
            return;

        try
        {
            var deadLetterDir = !string.IsNullOrEmpty(_settings.DeadLetterPath)
                ? _settings.DeadLetterPath
                : Path.Combine(_settings.PendingPath, "dead-letter");

            Directory.CreateDirectory(deadLetterDir);
            var deadLetterPath = Path.Combine(deadLetterDir, $"{videoId}{ext}");
            File.Move(pendingPath, deadLetterPath, overwrite: true);
            _logger.LogInformation("Moved failed video {VideoId} to dead-letter: {Path}", videoId, deadLetterPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to move video {VideoId} to dead-letter", videoId);
            CleanupPendingFile(pendingPath);
        }
    }

    private void CleanupPendingFile(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete pending file: {Path}", path);
        }
    }

    private async Task SendEmailSafeAsync(string toEmail, string title, string videoId, bool success)
    {
        try
        {
            await SendEmailAsync(toEmail, title, videoId, success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send {Type} email for video {VideoId}",
                success ? "success" : "failure", videoId);
        }
    }

    private static async Task RunProcessAsync(string fileName, string arguments, CancellationToken ct)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.PriorityClass = ProcessPriorityClass.BelowNormal;

        // Read output to avoid deadlocks
        await process.StandardOutput.ReadToEndAsync(ct);
        await process.StandardError.ReadToEndAsync(ct);

        await process.WaitForExitAsync(ct);

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Process {fileName} exited with code {process.ExitCode}");
        }
    }

    private async Task SendEmailAsync(string toEmail, string title, string videoId, bool success)
    {
        var videoLink = $"{_settings.SiteUrl}/Video/Watch?videoId={videoId}";

        using var mail = new MailMessage
        {
            From = new MailAddress(_settings.EmailFrom),
            Subject = success
                ? _settings.EmailSubject + title
                : _settings.EmailSubjectFailed + title,
            IsBodyHtml = true,
            Body = success
                ? string.Format(_settings.EmailBody, toEmail, videoLink, _settings.SiteUrl)
                : string.Format(_settings.EmailBodyFailed, toEmail, _settings.SiteUrl)
        };
        mail.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort);
        await client.SendMailAsync(mail);
    }
}
