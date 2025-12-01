using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class CreateInscripcionHandler : IRequestHandler<CreateInscripcionCommand, InscripcionDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateInscripcionHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InscripcionDto> Handle(CreateInscripcionCommand request, CancellationToken cancellationToken)
        {
            var inscripcion = _mapper.Map<Inscripcion>(request);
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<InscripcionDto>(inscripcion);
        }
    }
}