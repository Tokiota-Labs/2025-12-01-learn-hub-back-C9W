using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionCommand : IRequest<InscripcionDto>
    {
        public Guid AlumnoId { get; set; }
        public Guid CursoId { get; set; }
        public string PreferenciaHorario { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Valor por defecto
    }
}