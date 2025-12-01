using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class UpdateCourseCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}