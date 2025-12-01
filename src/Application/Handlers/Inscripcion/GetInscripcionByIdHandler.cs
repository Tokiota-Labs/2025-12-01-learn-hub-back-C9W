using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class GetInscripcionByIdHandler : IRequestHandler<GetInscripcionByIdQuery, InscripcionDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetInscripcionByIdHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InscripcionDto> Handle(GetInscripcionByIdQuery request, CancellationToken cancellationToken)
        {
            var inscripcion = await _context.Inscripciones.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            return _mapper.Map<InscripcionDto>(inscripcion);
        }
    }
}