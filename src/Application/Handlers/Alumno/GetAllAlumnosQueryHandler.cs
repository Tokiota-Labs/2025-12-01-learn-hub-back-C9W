using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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