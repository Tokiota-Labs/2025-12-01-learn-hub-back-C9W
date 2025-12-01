using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class GetCursoByIdQueryHandler : IRequestHandler<GetCursoByIdQuery, CursoDto>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetCursoByIdQueryHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CursoDto> Handle(GetCursoByIdQuery request, CancellationToken cancellationToken)
        {
            var curso = await _context.Cursos.FindAsync(new object[] { request.Id }, cancellationToken);
            return _mapper.Map<CursoDto>(curso);
        }
    }
}