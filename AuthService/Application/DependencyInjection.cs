using Application.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;


public static class DependencyInjection 
{ 
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration) 
    { 
        services.AddOptionsWithValidation<JwtOptions>(configuration, JwtOptions.SectionName);
        services.AddOptionsWithValidation<ReceptionUserOptions>(configuration, ReceptionUserOptions.SectionName);
        services.AddOptionsWithValidation<RedisOptions>(configuration, RedisOptions.SectionName);
        services.AddOptionsWithValidation<EmailOptions>(configuration, EmailOptions.SectionName);
        services.AddOptionsWithValidation<RabbitOptions>(configuration, RabbitOptions.SectionName);
        
        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection(RedisOptions.SectionName).Get<RedisOptions>();
            options.Configuration = redisConfig?.Configuration;
            options.InstanceName = redisConfig?.InstanceName;
        });
        
        services.AddScoped<IUserService, UserService>(); 
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICreatingReceptionService, CreatingReceptionService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRabbitService, RabbitService>();
    }

    public static void AddOptionsWithValidation<T>(this IServiceCollection services,
        IConfiguration configuration, string sectionName) where T : class
    {
        services.AddOptions<T>().Bind(configuration.GetSection(sectionName)).ValidateDataAnnotations().ValidateOnStart();
    }
}

