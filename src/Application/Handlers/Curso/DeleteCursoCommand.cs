using MediatR;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class DeleteCursoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}