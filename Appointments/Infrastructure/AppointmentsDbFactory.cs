using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class AppointmentsDbContextFactory : IDesignTimeDbContextFactory<AppointmentsDbContext>
{
    public AppointmentsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true) 
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .Build();
        
        var builder = new DbContextOptionsBuilder<AppointmentsDbContext>();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(connectionString);
        
        return new AppointmentsDbContext(builder.Options);
    }
}