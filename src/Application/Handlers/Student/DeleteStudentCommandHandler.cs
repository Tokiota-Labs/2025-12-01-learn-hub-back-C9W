using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteStudentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}