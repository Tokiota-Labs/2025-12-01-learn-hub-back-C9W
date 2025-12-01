using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCourseCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}