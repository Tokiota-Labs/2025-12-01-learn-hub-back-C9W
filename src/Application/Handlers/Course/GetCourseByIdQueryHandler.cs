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

            // Simulando que encontramos el curso con el ID solicitado
            if (request.Id == Guid.Empty)
                return null;

            return new CourseDto
            {
                Id = request.Id,
                Name = "Microservicios con .NET",
                Description = "Diseño e implementación de arquitecturas basadas en microservicios utilizando .NET, Docker y Kubernetes. Incluye patrones de comunicación, resiliencia y monitoreo.",
                StartDate = DateTime.UtcNow.AddDays(20),
                EndDate = DateTime.UtcNow.AddMonths(4)
            };
        }
    }
}