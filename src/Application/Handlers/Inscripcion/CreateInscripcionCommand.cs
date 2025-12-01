using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionCommand : IRequest<InscripcionDto>
    {
        public string Nombre { get; set; }
        // Agregar más propiedades según sea necesario
    }
}