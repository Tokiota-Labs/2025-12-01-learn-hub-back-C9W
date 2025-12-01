using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetAllCoursesQueryHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<CourseDto>>(courses);
        }
    }
}