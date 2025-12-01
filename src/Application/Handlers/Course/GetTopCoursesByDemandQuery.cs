using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetTopCoursesByDemandQuery : IRequest<List<CourseDto>>
    {
    }
}