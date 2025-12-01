using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class UpdateAlumnoCommandHandler : IRequestHandler<UpdateAlumnoCommand>
    {
        public Task<Unit> Handle(UpdateAlumnoCommand request, CancellationToken cancellationToken)
        {
            // Lógica para actualizar un alumno
            return Task.FromResult(Unit.Value);
        }
    }
}