using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetMostPopularCoursesQueryHandler : IRequestHandler<GetMostPopularCoursesQuery, List<PopularCourseDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetMostPopularCoursesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PopularCourseDto>> Handle(GetMostPopularCoursesQuery request, CancellationToken cancellationToken)
        {
            var popularCourses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Select(c => new PopularCourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    Category = c.Category,
                    Duration = c.Duration,
                    Modality = c.Modality,
                    TotalEnrollments = c.Enrollments.Count,
                    ActiveEnrollments = c.Enrollments.Count(e => e.Status == "Approved"),
                    AvailableSeats = c.AvailableSeats,
                    InstructorName = c.Instructor != null ? c.Instructor.Name : "Sin instructor",
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                })
                .OrderByDescending(c => c.TotalEnrollments)
                .Take(request.Count)
                .ToListAsync(cancellationToken);

            return popularCourses;
        }
    }
}
