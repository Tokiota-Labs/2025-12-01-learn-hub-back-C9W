using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class UpdateInscripcionValidator : AbstractValidator<UpdateInscripcionCommand>
    {
        public UpdateInscripcionValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            // Agregar más reglas de validación según sea necesario
        }
    }
}