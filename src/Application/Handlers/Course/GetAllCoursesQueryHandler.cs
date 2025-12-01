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

            return new List<CourseDto>
            {
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Desarrollo Web Full Stack",
                    Description = "Curso completo de desarrollo web con HTML5, CSS3, JavaScript, React y Node.js",
                    StartDate = DateTime.UtcNow.AddDays(15),
                    EndDate = DateTime.UtcNow.AddMonths(6)
                },
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Arquitectura de Software .NET",
                    Description = "Patrones de diseño, principios SOLID y arquitectura limpia en .NET",
                    StartDate = DateTime.UtcNow.AddDays(7),
                    EndDate = DateTime.UtcNow.AddMonths(3)
                },
                new CourseDto
                {
                    Id = Guid.NewGuid(),
                    Name = "DevOps y CI/CD",
                    Description = "Implementación de pipelines de CI/CD con Azure DevOps y GitHub Actions",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddMonths(2)
                }
            };
        }
    }
}