using MediatR;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class DeleteAlumnoCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}