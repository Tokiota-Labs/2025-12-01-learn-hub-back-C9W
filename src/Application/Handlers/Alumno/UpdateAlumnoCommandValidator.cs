using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class UpdateAlumnoCommandValidator : AbstractValidator<UpdateAlumnoCommand>
    {
        public UpdateAlumnoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es obligatorio");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El formato del email no es válido")
                .MaximumLength(100).WithMessage("El email no puede exceder los 100 caracteres");
        }
    }
}