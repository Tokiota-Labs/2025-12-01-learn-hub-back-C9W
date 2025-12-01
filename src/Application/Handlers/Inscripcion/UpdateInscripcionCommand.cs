using MediatR;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class UpdateInscripcionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Estado { get; set; }
        public string PreferenciaHorario { get; set; }
        public Guid? PagoId { get; set; }
    }
}