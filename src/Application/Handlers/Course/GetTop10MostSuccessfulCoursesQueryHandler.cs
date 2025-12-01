using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetTop10MostSuccessfulCoursesQueryHandler : IRequestHandler<GetTop10MostSuccessfulCoursesQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTop10MostSuccessfulCoursesQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetTop10MostSuccessfulCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Select(c => new 
                {
                    Course = c,
                    ApprovedEnrollmentsCount = c.Enrollments.Count(e => e.Status == EnrollmentStatus.Approved.ToString())
                })
                .OrderByDescending(x => x.ApprovedEnrollmentsCount)
                .Take(10)
                .Select(x => x.Course)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CourseDto>>(courses);
        }
    }
}