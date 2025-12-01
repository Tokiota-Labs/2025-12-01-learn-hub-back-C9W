using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class CreateCursoCommandHandler : IRequestHandler<CreateCursoCommand, CursoDto>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CreateCursoCommandHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CursoDto> Handle(CreateCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = _mapper.Map<Curso>(request);
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CursoDto>(curso);
        }
    }
}