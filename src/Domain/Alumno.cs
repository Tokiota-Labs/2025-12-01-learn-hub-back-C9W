namespace LearnHub.Back.Domain;

public class Alumno
{
    public Guid Id { get; set; }
    public string NombreCompleto { get; set; }
    public string CorreoElectronico { get; set; }
    public string NumeroTelefono { get; set; }
    public string DireccionPostal { get; set; }
    public string NivelEstudios { get; set; }
    public string OcupacionActual { get; set; }
    public string ExperienciaPrevia { get; set; }

    public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}