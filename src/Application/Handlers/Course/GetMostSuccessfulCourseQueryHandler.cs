using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetMostSuccessfulCourseQueryHandler : IRequestHandler<GetMostSuccessfulCourseQuery, MostSuccessfulCourseDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMostSuccessfulCourseQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MostSuccessfulCourseDto?> Handle(GetMostSuccessfulCourseQuery request, CancellationToken cancellationToken)
        {
            // Get the course with the most enrollments
            var mostSuccessfulCourse = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.Enrollments.Any()) // Only courses with enrollments
                .OrderByDescending(c => c.Enrollments.Count)
                .FirstOrDefaultAsync(cancellationToken);

            if (mostSuccessfulCourse == null)
            {
                return null;
            }

            var courseDto = _mapper.Map<CourseDto>(mostSuccessfulCourse);
            
            return new MostSuccessfulCourseDto
            {
                Course = courseDto,
                EnrollmentCount = mostSuccessfulCourse.Enrollments.Count
            };
        }
    }
}