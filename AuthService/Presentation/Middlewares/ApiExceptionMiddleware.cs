using Domain.Common;
using Newtonsoft.Json;

namespace Presentation.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseExceptionHandler ex)
        {
            _logger.LogWarning("Unhandled exception {Message}", ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled server exception {Message}", ex.Message);
            await HandleExceptionAsync(context, 500, "Server Error").ConfigureAwait(false);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int  statusCode, string message)
    {
        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = statusCode,
            Message = message
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsync(result);
    }
}