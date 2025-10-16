using Application.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;


public static class DependencyInjection 
{ 
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) 
    { 
        services.Configure<JwtOptions>(configuration.GetSection("Jwt")); 
        services.Configure<ReceptionUserOptions>(configuration.GetSection("Reception"));
        services.Configure<RedisOptions>(configuration.GetSection("Redis"));
        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));
        
        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection("Redis").Get<RedisOptions>();
            options.Configuration = redisConfig?.Configuration;
            options.InstanceName = redisConfig?.InstanceName;
        });
        
        services.AddScoped<IUserService, UserService>(); 
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICreatingReceptionService, CreatingReceptionService>();
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
        
    }
}
