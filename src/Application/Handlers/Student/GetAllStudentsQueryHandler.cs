using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
    {
        public Task<List<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            // Logic to get all students
            return Task.FromResult(new List<StudentDto>());
        }
    }
}