using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class DeleteEnrollmentHandler : IRequestHandler<DeleteEnrollmentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEnrollmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (enrollment != null)
            {
                // If the enrollment was active, increase available seats when deleting
                if (enrollment.Status == "Active")
                {
                    enrollment.Course.AvailableSeats++;
                }

                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}