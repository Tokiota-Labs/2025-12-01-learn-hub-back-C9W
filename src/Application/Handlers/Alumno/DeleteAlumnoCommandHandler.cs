using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class DeleteAlumnoCommandHandler : IRequestHandler<DeleteAlumnoCommand>
    {
        public Task<Unit> Handle(DeleteAlumnoCommand request, CancellationToken cancellationToken)
        {
            // Lógica para eliminar un alumno
            return Task.FromResult(Unit.Value);
        }
    }
}