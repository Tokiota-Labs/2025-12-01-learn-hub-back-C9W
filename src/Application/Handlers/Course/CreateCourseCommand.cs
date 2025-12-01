using MediatR;
using LearnHub.Back.Application.DTOs;
using System;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class CreateCourseCommand : IRequest<CourseDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}