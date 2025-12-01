using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class UpdateCursoCommandValidator : AbstractValidator<UpdateCursoCommand>
    {
        public UpdateCursoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es obligatoria.");
            RuleFor(x => x.FechaInicio).LessThan(x => x.FechaFin).WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");
        }
    }
}