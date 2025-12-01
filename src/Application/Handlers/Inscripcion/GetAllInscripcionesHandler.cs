using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class GetAllInscripcionesHandler : IRequestHandler<GetAllInscripcionesQuery, List<InscripcionDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllInscripcionesHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InscripcionDto>> Handle(GetAllInscripcionesQuery request, CancellationToken cancellationToken)
        {
            var inscripciones = await _context.Inscripciones.ToListAsync(cancellationToken);
            return _mapper.Map<List<InscripcionDto>>(inscripciones);
        }
    }
}