using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            //var cursos = await _context.Cursos.ToListAsync(cancellationToken);
            //return _mapper.Map<List<CursoDto>>(cursos);

            return new List<CursoDto>()
            {
                new CursoDto()
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Curso 1",
                    Descripcion = "Descrição do curso 1",
                    FechaInicio = DateTime.UtcNow,
                    FechaFin = DateTime.UtcNow.AddMonths(1)
                },
                new CursoDto()
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Curso 2",
                    Descripcion = "Descrição do curso 2",
                    FechaInicio = DateTime.UtcNow,
                    FechaFin = DateTime.UtcNow.AddMonths(4)
                }
            };
        }
    }
}