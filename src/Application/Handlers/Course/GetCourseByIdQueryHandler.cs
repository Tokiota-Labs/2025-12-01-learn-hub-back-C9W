using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetCourseByIdQueryHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            
            return _mapper.Map<CourseDto>(course);
        }
    }
}