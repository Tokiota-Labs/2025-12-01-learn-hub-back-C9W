using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetTopSuccessfulCoursesQueryHandler : IRequestHandler<GetTopSuccessfulCoursesQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTopSuccessfulCoursesQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetTopSuccessfulCoursesQuery request, CancellationToken cancellationToken)
        {
            var topCourses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.Enrollments.Any(e => e.Status == "Approved"))
                .OrderByDescending(c => c.Enrollments.Count(e => e.Status == "Approved"))
                .Take(10)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CourseDto>>(topCourses);
        }
    }
}