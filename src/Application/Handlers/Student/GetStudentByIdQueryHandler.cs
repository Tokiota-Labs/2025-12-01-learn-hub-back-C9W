using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        public Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            // Logic to get a student by ID
            return Task.FromResult(new StudentDto());
        }
    }
}