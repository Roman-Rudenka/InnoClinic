using Microsoft.OpenApi.Models;

namespace Presentation.Extensions;

public static class SwaggerConfiguration
{
    public static void AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AuthService",
                Version = "v1",
                Description = "AuthService API"
            });
        });
    }
}