using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class UpdateInscripcionCommandValidator : AbstractValidator<UpdateInscripcionCommand>
    {
        public UpdateInscripcionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es obligatorio");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio")
                .Must(x => x == "Pendiente" || x == "Aprobada" || x == "Rechazada")
                .WithMessage("El estado debe ser 'Pendiente', 'Aprobada' o 'Rechazada'");

            RuleFor(x => x.PreferenciaHorario)
                .NotEmpty().WithMessage("La preferencia de horario es obligatoria")
                .MaximumLength(50).WithMessage("La preferencia de horario no puede exceder los 50 caracteres");
        }
    }
}