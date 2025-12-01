using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class GetAllAlumnosQueryHandler : IRequestHandler<GetAllAlumnosQuery, List<AlumnoDto>>
    {
        public Task<List<AlumnoDto>> Handle(GetAllAlumnosQuery request, CancellationToken cancellationToken)
        {
            // Lógica para obtener todos los alumnos
            return Task.FromResult(new List<AlumnoDto>());
        }
    }
}