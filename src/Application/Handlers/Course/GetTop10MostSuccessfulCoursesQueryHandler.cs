using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnHub.Back.Application.Handlers.Course
{
    /// <summary>
    /// Handler for retrieving the top 10 most successful courses
    /// Courses are ranked by the number of approved enrollments
    /// </summary>
    public class GetTop10MostSuccessfulCoursesQueryHandler : IRequestHandler<GetTop10MostSuccessfulCoursesQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTop10MostSuccessfulCoursesQueryHandler> _logger;

        public GetTop10MostSuccessfulCoursesQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetTop10MostSuccessfulCoursesQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles the query to get top 10 most successful courses
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of top 10 most successful courses</returns>
        public async Task<List<CourseDto>> Handle(GetTop10MostSuccessfulCoursesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving top 10 most successful courses");

            try
            {
                // Query courses with approved enrollments count
                var courses = await _context.Courses
                    .Include(c => c.Instructor)
                    .Include(c => c.Enrollments)
                    .Select(c => new 
                    {
                        Course = c,
                        ApprovedEnrollmentsCount = c.Enrollments.Count(e => e.Status == "Approved")
                    })
                    .OrderByDescending(x => x.ApprovedEnrollmentsCount)
                    .Take(10)
                    .Select(x => x.Course)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Successfully retrieved {Count} courses", courses.Count);

                return _mapper.Map<List<CourseDto>>(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving top 10 most successful courses");
                throw;
            }
        }
    }
}
