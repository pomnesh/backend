using Serilog;
using ILogger = Serilog.ILogger;

namespace Pomnesh.API.Middlewares;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly long _thresholdMilliseconds;

    public PerformanceMiddleware(RequestDelegate next, long thresholdMilliseconds = 1000)
    {
        _next = next;
        _logger = Log.ForContext<PerformanceMiddleware>();
        _thresholdMilliseconds = thresholdMilliseconds;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        
        try
        {
            await _next(context);
        }
        finally
        {
            var duration = DateTime.UtcNow - startTime;
            if (duration.TotalMilliseconds > _thresholdMilliseconds)
            {
                _logger.Warning(
                    "Slow request detected: {Method} {Path} took {Duration}ms (threshold: {Threshold}ms)",
                    context.Request.Method,
                    context.Request.Path,
                    duration.TotalMilliseconds,
                    _thresholdMilliseconds);
            }
            else
            {
                _logger.Debug(
                    "Request completed: {Method} {Path} took {Duration}ms",
                    context.Request.Method,
                    context.Request.Path,
                    duration.TotalMilliseconds);
            }
        }
    }
} 