using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Unit>
    {
        public Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // Logic to update a student
            return Task.FromResult(Unit.Value);
        }
    }
}