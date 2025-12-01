using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class UpdateCursoCommandValidator : AbstractValidator<UpdateCursoCommand>
    {
        public UpdateCursoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es obligatorio");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");

            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria")
                .LessThan(x => x.FechaFin).WithMessage("La fecha de inicio debe ser anterior a la fecha de fin");

            RuleFor(x => x.FechaFin)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria");
        }
    }
}