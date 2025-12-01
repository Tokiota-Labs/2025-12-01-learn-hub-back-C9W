using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetTopCoursesByDemandQueryHandler : IRequestHandler<GetTopCoursesByDemandQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTopCoursesByDemandQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetTopCoursesByDemandQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .Include(c => c.Enrollments)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(10)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CourseDto>>(courses);
        }
    }
}