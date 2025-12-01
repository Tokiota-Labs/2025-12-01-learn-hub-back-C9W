using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCourseCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            //var course = await _context.Courses.FindAsync(new object[] { request.Id }, cancellationToken);
            //_context.Courses.Remove(course);
            //await _context.SaveChangesAsync(cancellationToken);

            // Simulando la eliminación de un curso
            // En un escenario real, aquí se validaría la existencia del curso y sus dependencias
            if (request.Id == Guid.Empty)
                throw new KeyNotFoundException("Course not found");

            // Simulando que el curso tiene matrículas activas
            if (request.Id == Guid.Parse("11111111-1111-1111-1111-111111111111"))
                throw new InvalidOperationException("Cannot delete course with active enrollments");

            // Simulando una eliminación exitosa
            return Unit.Value;
        }
    }
}