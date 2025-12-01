using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class GetAlumnoByIdQuery : IRequest<AlumnoDto>
    {
        public Guid Id { get; set; }
    }
}