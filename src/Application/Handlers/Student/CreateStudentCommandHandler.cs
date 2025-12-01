using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<Domain.Student>(request);
            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<StudentDto>(student);
        }
    }
}