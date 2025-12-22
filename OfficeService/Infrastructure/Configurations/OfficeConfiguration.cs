using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Address).IsRequired().HasMaxLength(250);
        builder.Property(o => o.RegistryPhoneNumber).HasMaxLength(20);
        builder.ToTable("Offices");
    }
}