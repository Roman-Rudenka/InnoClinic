using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ReceptionistConfiguration : IEntityTypeConfiguration<Receptionist>
{
    public void Configure(EntityTypeBuilder<Receptionist> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.MiddleName)
            .HasMaxLength(100);

        builder.Property(r => r.BirthDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(r => r.IsLinkedToAccount)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.AccountId)
            .IsRequired();
        
        builder.Property(r => r.OfficeId)
            .IsRequired();

        builder.ToTable("Receptionists");
    }
}