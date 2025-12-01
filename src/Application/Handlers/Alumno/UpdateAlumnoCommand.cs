using MediatR;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class UpdateAlumnoCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
    }
}