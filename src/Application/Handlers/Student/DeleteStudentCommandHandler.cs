using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Unit>
    {
        public Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            // Logic to delete a student
            return Task.FromResult(Unit.Value);
        }
    }
}