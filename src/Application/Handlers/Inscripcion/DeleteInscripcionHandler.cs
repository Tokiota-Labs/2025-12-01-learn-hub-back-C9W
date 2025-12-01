using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class DeleteInscripcionHandler : IRequestHandler<DeleteInscripcionCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteInscripcionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteInscripcionCommand request, CancellationToken cancellationToken)
        {
            var inscripcion = await _context.Inscripciones.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}