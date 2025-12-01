using Microsoft.EntityFrameworkCore;
using LearnHub.Back.Domain;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace LearnHub.Back.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure indexes
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<Instructor>()
            .HasIndex(i => i.Name)
            .IsUnique();

        // Configure relationships
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Enrollment)
            .WithOne(e => e.Payment)
            .HasForeignKey<Payment>(p => p.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}