using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    /// <summary>
    /// Query to get the most successful course (by enrollment count)
    /// </summary>
    public class GetMostSuccessfulCourseQuery : IRequest<MostSuccessfulCourseDto?>
    {
    }
}