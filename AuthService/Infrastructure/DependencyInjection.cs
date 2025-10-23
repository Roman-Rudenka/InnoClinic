using Application;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidation<DbOptions>(configuration, DbOptions.SectionName);

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
            options.UseNpgsql(dbOptions.DefaultConnection);
        });
        services.AddIdentity<User, IdentityRole<Guid>>((options) =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IRoleSeeder, RoleSeeder>();
        services.AddScoped<ITokenRepository, TokenRepository>();
    }
}