using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class CreateAlumnoCommandValidator : AbstractValidator<CreateAlumnoCommand>
    {
        public CreateAlumnoCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido es obligatorio.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("El email es obligatorio y debe ser válido.");
        }
    }
}