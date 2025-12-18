using System.Diagnostics;

namespace Presentation.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path;
        var endpoint = context.GetEndpoint()?.DisplayName ?? "Unknown endpoint";

        logger.LogInformation("Request started: {Method} {Path} ({Endpoint})", method, path, endpoint);

        await next(context);

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;

        logger.LogInformation("Request completed: {Method} {Path} | Status: {StatusCode} | Time: {Elapsed}ms",
            method, path, statusCode, stopwatch.ElapsedMilliseconds);
    }
}