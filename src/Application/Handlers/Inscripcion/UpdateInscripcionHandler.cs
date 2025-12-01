using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class UpdateInscripcionHandler : IRequestHandler<UpdateInscripcionCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateInscripcionHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInscripcionCommand request, CancellationToken cancellationToken)
        {
            var inscripcion = await _context.Inscripciones.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            _mapper.Map(request, inscripcion);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}