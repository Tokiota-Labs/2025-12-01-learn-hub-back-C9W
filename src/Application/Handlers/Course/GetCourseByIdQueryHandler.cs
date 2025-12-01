using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;

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
            //var course = await _context.Courses.FindAsync(new object[] { request.Id }, cancellationToken);
            //return _mapper.Map<CourseDto>(course);

            return new CourseDto()
            {
                Id = Guid.NewGuid(),
                Name = "Course 3",
                Description = "Description of course 3",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(6)
            };
        }
    }
}