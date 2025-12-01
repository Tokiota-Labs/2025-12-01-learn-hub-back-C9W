using MediatR;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class DeleteAlumnoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}