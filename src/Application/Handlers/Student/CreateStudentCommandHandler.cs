using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        public Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            // Logic to create a new student
            return Task.FromResult(new StudentDto());
        }
    }
}