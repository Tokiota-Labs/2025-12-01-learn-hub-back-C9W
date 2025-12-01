using Microsoft.EntityFrameworkCore;
using LearnHub.Back.Domain;

namespace LearnHub.Back.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }
    public DbSet<Instructor> Instructores { get; set; }
    public DbSet<Pago> Pagos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // You can add additional configuration here using modelBuilder
        // For example: relationships, indexes, constraints, etc.
    }
}