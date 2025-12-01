using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionValidator : AbstractValidator<CreateInscripcionCommand>
    {
        public CreateInscripcionValidator()
        {
            // Agregar más reglas de validación según sea necesario
        }
    }
}