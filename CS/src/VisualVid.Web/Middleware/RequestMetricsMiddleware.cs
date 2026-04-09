using System.Diagnostics;

namespace VisualVid.Web.Middleware;

public class RequestMetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestMetricsMiddleware> _logger;

    private static long _totalRequests;
    private static long _totalErrors;
    private static long _authFailures;
    private static long _uploadRequests;
    private static long _uploadFailures;
    private static double _totalLatencyMs;

    public RequestMetricsMiddleware(RequestDelegate next, ILogger<RequestMetricsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        Interlocked.Increment(ref _totalRequests);

        var path = context.Request.Path.Value ?? "";
        var isUpload = path.Contains("/Video/Upload", StringComparison.OrdinalIgnoreCase) &&
                       context.Request.Method == "POST";
        if (isUpload)
            Interlocked.Increment(ref _uploadRequests);

        try
        {
            await _next(context);

            sw.Stop();
            var elapsed = sw.Elapsed.TotalMilliseconds;
            InterlockedAdd(ref _totalLatencyMs, elapsed);

            var statusCode = context.Response.StatusCode;

            if (statusCode >= 500)
                Interlocked.Increment(ref _totalErrors);

            if (statusCode == 401 || statusCode == 403)
                Interlocked.Increment(ref _authFailures);

            if (isUpload && statusCode >= 400)
                Interlocked.Increment(ref _uploadFailures);

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs:F1}ms",
                context.Request.Method,
                path,
                statusCode,
                elapsed);
        }
        catch (Exception ex)
        {
            sw.Stop();
            Interlocked.Increment(ref _totalErrors);

            if (isUpload)
                Interlocked.Increment(ref _uploadFailures);

            _logger.LogError(ex,
                "HTTP {Method} {Path} threw exception after {ElapsedMs:F1}ms",
                context.Request.Method,
                path,
                sw.Elapsed.TotalMilliseconds);

            throw;
        }
    }

    private static void InterlockedAdd(ref double location, double value)
    {
        double newCurrentValue = location;
        while (true)
        {
            double currentValue = newCurrentValue;
            double newValue = currentValue + value;
            newCurrentValue = Interlocked.CompareExchange(ref location, newValue, currentValue);
            if (newCurrentValue == currentValue)
                break;
        }
    }

    public static MetricsSnapshot GetSnapshot() => new()
    {
        TotalRequests = Interlocked.Read(ref _totalRequests),
        TotalErrors = Interlocked.Read(ref _totalErrors),
        AuthFailures = Interlocked.Read(ref _authFailures),
        UploadRequests = Interlocked.Read(ref _uploadRequests),
        UploadFailures = Interlocked.Read(ref _uploadFailures),
        AverageLatencyMs = _totalRequests > 0 ? _totalLatencyMs / _totalRequests : 0
    };
}

public class MetricsSnapshot
{
    public long TotalRequests { get; init; }
    public long TotalErrors { get; init; }
    public long AuthFailures { get; init; }
    public long UploadRequests { get; init; }
    public long UploadFailures { get; init; }
    public double AverageLatencyMs { get; init; }
}
