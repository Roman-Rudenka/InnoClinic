namespace Presentation.Middlewares;

public class CookieAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("access_token", out var token) && !string.IsNullOrEmpty(token))
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers.Authorization = $"Bearer {token}";
            }
        }
        
        await next(context);
    }
}