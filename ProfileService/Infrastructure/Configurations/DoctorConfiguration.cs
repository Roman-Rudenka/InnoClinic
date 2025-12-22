using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.MiddleName)
            .HasMaxLength(100);

        builder.Property(d => d.BirthDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(d => d.IsLinkedToAccount)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(d => d.AccountId)
            .IsRequired();
        
        builder.Property(d => d.StartWorkingDate)
            .IsRequired();
        
        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>(); 

        builder.Property(d => d.SpecializationId)
            .IsRequired();

        builder.Property(d => d.OfficeId)
            .IsRequired();

        builder.ToTable("Doctors");
    }
    
}