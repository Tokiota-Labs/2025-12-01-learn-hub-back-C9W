using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class DeleteCursoCommandHandler : IRequestHandler<DeleteCursoCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCursoCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = await _context.Cursos.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}