using Application.Interfaces;
using Infrastructure.Common;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbOptions>(options =>
        {
            configuration.GetSection(nameof(DbOptions)).Bind(options);
        });
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
            options.UseNpgsql(dbOptions.DefaultConnection);
        });

        services.AddScoped(typeof(IProfileRepository<>), typeof(ProfileRepository<>));
        
        return services;
    }
}