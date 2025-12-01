using MediatR;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class DeleteInscripcionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}