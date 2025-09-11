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
        
        services.AddScoped<IUserService, UserService>(); 
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
        
    }
}
