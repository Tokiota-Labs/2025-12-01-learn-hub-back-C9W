using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionCommandValidator : AbstractValidator<CreateInscripcionCommand>
    {
        public CreateInscripcionCommandValidator()
        {
            RuleFor(x => x.AlumnoId)
                .NotEmpty().WithMessage("El ID del alumno es obligatorio");

            RuleFor(x => x.CursoId)
                .NotEmpty().WithMessage("El ID del curso es obligatorio");

            RuleFor(x => x.PreferenciaHorario)
                .NotEmpty().WithMessage("La preferencia de horario es obligatoria")
                .MaximumLength(50).WithMessage("La preferencia de horario no puede exceder los 50 caracteres");

            RuleFor(x => x.Estado)
                .Must(x => x == "Pendiente" || x == "Aprobada" || x == "Rechazada")
                .WithMessage("El estado debe ser 'Pendiente', 'Aprobada' o 'Rechazada'");
        }
    }
}