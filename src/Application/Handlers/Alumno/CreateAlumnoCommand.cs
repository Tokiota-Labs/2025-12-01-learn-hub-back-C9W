using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class CreateAlumnoCommand : IRequest<AlumnoDto>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
    }
}