using LearnHub.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnHub.Back.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);
            
        builder.Property(s => s.PostalAddress)
            .HasMaxLength(500);

        builder.Property(s => s.EducationLevel)
            .HasMaxLength(100);

        builder.Property(s => s.CurrentOccupation)
            .HasMaxLength(200);

        builder.Property(s => s.PreviousExperience)
            .HasMaxLength(1000);
    }
}