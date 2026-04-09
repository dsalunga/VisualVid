namespace VisualVid.Encoder;

public class EncoderSettings
{
    public string DatabaseProvider { get; set; } = "SqlServer";
    public string ConnectionString { get; set; } = "";
    public string FfmpegPath { get; set; } = "ffmpeg";
    public string VideoStoragePath { get; set; } = "";
    public string PendingPath { get; set; } = "";
    public string DeadLetterPath { get; set; } = "";
    public string OutputFormat { get; set; } = "mp4";
    public string ThumbnailTimeOffset { get; set; } = "00:00:05";

    // Retry settings
    public int MaxRetries { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 60;

    // Email settings
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public string EmailFrom { get; set; } = "noreply@visualvid.com";
    public string EmailSubject { get; set; } = "Your video is now live: ";
    public string EmailSubjectFailed { get; set; } = "Video encoding failed: ";
    public string EmailBody { get; set; } = "<p>Dear <strong>{0}</strong>,</p><p>Your video is now live on VisualVid. <a href='{1}'>Click here to view it</a>.</p><p><strong>The VisualVid Team</strong></p>";
    public string EmailBodyFailed { get; set; } = "<p>Dear <strong>{0}</strong>,</p><p>Unfortunately, we were unable to process your video. Please try uploading again.</p><p><strong>The VisualVid Team</strong></p>";
    public string SiteUrl { get; set; } = "http://localhost:5000";

    public int PollIntervalSeconds { get; set; } = 30;
}
