using LearnHub.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnHub.Back.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Price)
            .HasPrecision(10, 2);

        builder.Property(c => c.Prerequisites)
            .HasMaxLength(500);

        builder.Property(c => c.Modality)
            .HasMaxLength(50);

        builder.Property(c => c.IncludedMaterials)
            .HasMaxLength(500);

        builder.Property(c => c.Certification)
            .HasMaxLength(200);

        builder.Property(c => c.Location)
            .HasMaxLength(200);

        builder.Property(c => c.Category)
            .HasMaxLength(100);

        // Configure Schedule as a converted JSON array
        builder.Property(c => c.Schedule)
            .HasConversion(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList()
            );
    }
}