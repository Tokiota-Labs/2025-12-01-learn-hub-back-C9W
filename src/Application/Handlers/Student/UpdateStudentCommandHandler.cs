using AutoMapper;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (student != null)
            {
                _mapper.Map(request, student);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}