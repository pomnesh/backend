using Pomnesh.Application.Exceptions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Pomnesh.API.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<ApiExceptionMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseApiException ex)
        {
            _logger.Warning("User encountered expected error: {Error} with status code {StatusCode} at path {Path}", 
                ex.Description, 
                ex.StatusCode,
                context.Request.Path);
                
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = ex.StatusCode,
                Error = ex.Description // Only uses the predefined description
            });
        }
    }
    
}