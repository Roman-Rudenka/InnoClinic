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
        services.Configure<RabbitOptions>(configuration.GetSection("Rabbit"));
        
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IReceptionService, ReceptionService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddHostedService<RabbitService>();
    }
}