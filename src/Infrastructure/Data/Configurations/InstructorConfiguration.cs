using LearnHub.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnHub.Back.Infrastructure.Data.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Biography)
            .HasMaxLength(1000);
    }
}