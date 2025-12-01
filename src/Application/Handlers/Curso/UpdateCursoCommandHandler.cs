using AutoMapper;
using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class UpdateCursoCommandHandler : IRequestHandler<UpdateCursoCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UpdateCursoCommandHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCursoCommand request, CancellationToken cancellationToken)
        {
            //var curso = await _context.Cursos.FindAsync(new object[] { request.Id }, cancellationToken);
            //_mapper.Map(request, curso);
            //await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}