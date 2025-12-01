using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetCoursesWithLeastDemandQueryHandler : IRequestHandler<GetCoursesWithLeastDemandQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCoursesWithLeastDemandQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetCoursesWithLeastDemandQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .OrderBy(c => c.Enrollments.Count)
                .Take(request.Count)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CourseDto>>(courses);
        }
    }
}
