using AutoMapper;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class UpdateEnrollmentHandler : IRequestHandler<UpdateEnrollmentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEnrollmentHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (enrollment != null)
            {
                var oldStatus = enrollment.Status;
                _mapper.Map(request, enrollment);

                // If status changed from Active to Dropped, increase available seats
                if (oldStatus == "Active" && enrollment.Status == "Dropped")
                {
                    enrollment.Course.AvailableSeats++;
                }
                // If status changed from Dropped to Active, decrease available seats
                else if (oldStatus == "Dropped" && enrollment.Status == "Active")
                {
                    if (enrollment.Course.AvailableSeats > 0)
                    {
                        enrollment.Course.AvailableSeats--;
                    }
                    else
                    {
                        throw new InvalidOperationException("No available seats in the course");
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}