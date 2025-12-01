using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class CreateEnrollmentHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateEnrollmentHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EnrollmentDto> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            // First check if there are available seats and get course details
            var course = await _context.Courses.FindAsync(new object[] { request.CourseId }, cancellationToken);
            if (course != null && course.AvailableSeats > 0)
            {
                var enrollment = _mapper.Map<Domain.Enrollment>(request);
                enrollment.EnrollmentDate = DateTime.UtcNow;
                
                // Set default schedule preference if none provided
                if (string.IsNullOrEmpty(enrollment.SchedulePreference) && course.Schedule.Any())
                {
                    enrollment.SchedulePreference = course.Schedule[0];
                }
                else if (string.IsNullOrEmpty(enrollment.SchedulePreference))
                {
                    enrollment.SchedulePreference = "Default Schedule";
                }
                
                _context.Enrollments.Add(enrollment);
                
                // Update available seats
                course.AvailableSeats--;
                
                await _context.SaveChangesAsync(cancellationToken);

                // Load the complete enrollment with navigation properties for the DTO
                var completeEnrollment = await _context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .FirstAsync(e => e.Id == enrollment.Id, cancellationToken);

                return _mapper.Map<EnrollmentDto>(completeEnrollment);
            }

            throw new InvalidOperationException("No available seats in the course");
        }
    }
}