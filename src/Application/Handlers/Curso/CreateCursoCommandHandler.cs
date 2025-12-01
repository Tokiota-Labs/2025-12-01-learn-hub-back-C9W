using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;

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
            //var curso = _mapper.Map<Domain.Curso>(request);
            //_context.Cursos.Add(curso);
            //await _context.SaveChangesAsync(cancellationToken);
            //return _mapper.Map<CursoDto>(curso);

            return new CursoDto()
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin
            };
        }
    }
}