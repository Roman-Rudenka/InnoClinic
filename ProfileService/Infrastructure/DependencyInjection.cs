using Application.Interfaces;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        

        services.AddScoped(typeof(IProfileRepository<>), typeof(ProfileRepository<>));
        
        return services;
    }
}