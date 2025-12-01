namespace LearnHub.Back.Application.DTOs
{
    public class AlumnoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
    }
}