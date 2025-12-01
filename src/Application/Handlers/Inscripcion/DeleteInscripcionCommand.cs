using MediatR;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class DeleteInscripcionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}