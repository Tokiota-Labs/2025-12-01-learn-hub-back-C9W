using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetCoursesWithLeastDemandQuery : IRequest<List<CourseDto>>
    {
        public int Count { get; set; } = 10;
    }
}
