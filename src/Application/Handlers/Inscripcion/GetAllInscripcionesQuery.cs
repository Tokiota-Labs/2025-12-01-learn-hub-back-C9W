using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Inscripcion
{
    public class GetAllInscripcionesQuery : IRequest<List<InscripcionDto>>
    {
    }
}