using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Patient>  Patients { get; set; }
    //public DbSet<Doctor>  Doctors { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}