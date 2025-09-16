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
        
        services.AddScoped<IUserService, UserService>(); 
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICreatingReceptionService, CreatingReceptionService>();
        
        return services;
        
    }
}
