using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ResultConfiguration : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder.ToTable("Results");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Conclusion).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.Recommendation).IsRequired().HasMaxLength(2000);
    }
}