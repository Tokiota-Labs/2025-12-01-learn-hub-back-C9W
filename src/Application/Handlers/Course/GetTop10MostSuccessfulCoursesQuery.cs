using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    /// <summary>
    /// Query to retrieve the top 10 most successful courses
    /// Success is measured by the number of approved enrollments
    /// </summary>
    public class GetTop10MostSuccessfulCoursesQuery : IRequest<List<CourseDto>>
    {
        // Empty request following MediatR pattern for parameterless queries
    }
}
