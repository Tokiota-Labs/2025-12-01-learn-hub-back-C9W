using System;

public class Inscripcion
{
    public Guid Id { get; set; }
    public Guid AlumnoId { get; set; }
    public Alumno Alumno { get; set; }
    public Guid CursoId { get; set; }
    public Curso Curso { get; set; }
    public DateTime FechaInscripcion { get; set; }
    public string Estado { get; set; } // Aprobada, Rechazada, Pendiente

    public Guid PagoId { get; set; }
    public Pago Pago { get; set; }
}