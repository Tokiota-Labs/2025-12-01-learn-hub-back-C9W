using MediatR;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class UpdateInscripcionCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        // Agregar más propiedades según sea necesario
    }
}