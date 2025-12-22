using System.Reflection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Patient>  Patients { get; set; }
    public DbSet<Doctor>  Doctors { get; set; }
    public DbSet<Receptionist>  Receptionists { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext)) ?? Assembly.GetExecutingAssembly());
    }
}