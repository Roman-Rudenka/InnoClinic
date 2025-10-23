using Application.Exceptions;
using Newtonsoft.Json;

namespace Presentation.Middlewares;

public class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            logger.LogWarning("Unhandled exception {Message}", ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError("Unhandled server exception {Message}", ex.Message);
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