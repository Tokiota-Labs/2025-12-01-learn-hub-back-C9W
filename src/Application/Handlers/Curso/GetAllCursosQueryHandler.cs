using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class GetAllCursosQueryHandler : IRequestHandler<GetAllCursosQuery, List<CursoDto>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetAllCursosQueryHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CursoDto>> Handle(GetAllCursosQuery request, CancellationToken cancellationToken)
        {
            var cursos = await _context.Cursos.ToListAsync(cancellationToken);
            return _mapper.Map<List<CursoDto>>(cursos);
        }
    }
}