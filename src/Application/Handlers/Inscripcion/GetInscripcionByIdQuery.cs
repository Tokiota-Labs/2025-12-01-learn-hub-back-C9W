using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class GetInscripcionByIdQuery : IRequest<InscripcionDto>
    {
        public Guid Id { get; set; }
    }
}