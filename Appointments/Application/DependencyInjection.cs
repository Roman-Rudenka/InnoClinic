using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}