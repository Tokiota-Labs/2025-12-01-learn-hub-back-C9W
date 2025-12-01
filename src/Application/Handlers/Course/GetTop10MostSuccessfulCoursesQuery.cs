using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetTop10MostSuccessfulCoursesQuery : IRequest<List<CourseDto>>
    {
    }
}