using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class GetAlumnoByIdQueryHandler : IRequestHandler<GetAlumnoByIdQuery, AlumnoDto>
    {
        public Task<AlumnoDto> Handle(GetAlumnoByIdQuery request, CancellationToken cancellationToken)
        {
            // Lógica para obtener un alumno por ID
            return Task.FromResult(new AlumnoDto());
        }
    }
}