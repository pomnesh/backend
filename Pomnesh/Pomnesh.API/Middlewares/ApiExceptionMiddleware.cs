using Pomnesh.Application.Exceptions;

namespace Pomnesh.API.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseApiException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = ex.StatusCode,
                Error = ex.Description // Only uses the predefined description
            });
        }
    }
    
}