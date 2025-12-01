using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Course
{
    /// <summary>
    /// Query to get the top 10 most popular courses based on enrollment count
    /// </summary>
    public class GetMostPopularCoursesQuery : IRequest<List<PopularCourseDto>>
    {
        /// <summary>
        /// Number of courses to return (default: 10)
        /// </summary>
        public int Count { get; set; } = 10;
    }
}
