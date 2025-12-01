using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionValidator : AbstractValidator<CreateInscripcionCommand>
    {
        public CreateInscripcionValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            // Agregar más reglas de validación según sea necesario
        }
    }
}