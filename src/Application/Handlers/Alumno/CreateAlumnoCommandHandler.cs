using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class CreateAlumnoCommandHandler : IRequestHandler<CreateAlumnoCommand, AlumnoDto>
    {
        public Task<AlumnoDto> Handle(CreateAlumnoCommand request, CancellationToken cancellationToken)
        {
            // Lógica para crear un nuevo alumno
            return Task.FromResult(new AlumnoDto());
        }
    }
}