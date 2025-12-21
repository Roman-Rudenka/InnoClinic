using System.Reflection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext( DbContextOptions<AppDbContext> options ) : DbContext(options)
{
    public DbSet<Office> Offices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly((Assembly.GetAssembly(typeof(AppDbContext))) ?? Assembly.GetExecutingAssembly()); 
    }
}