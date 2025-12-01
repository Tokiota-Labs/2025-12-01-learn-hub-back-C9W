using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class DeleteCursoCommandHandler : IRequestHandler<DeleteCursoCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCursoCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCursoCommand request, CancellationToken cancellationToken)
        {
            //var curso = await _context.Cursos.FindAsync(new object[] { request.Id }, cancellationToken);
            //_context.Cursos.Remove(curso);
            //await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}