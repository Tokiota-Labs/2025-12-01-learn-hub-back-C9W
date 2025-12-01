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
            //var courses = await _context.Courses.ToListAsync(cancellationToken);
            //return _mapper.Map<List<CourseDto>>(courses);

            return new List<CourseDto>()
            {
                new CourseDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Course 1",
                    Description = "Description of course 1",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1)
                },
                new CourseDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Course 2",
                    Description = "Description of course 2",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(4)
                }
            };
        }
    }
}