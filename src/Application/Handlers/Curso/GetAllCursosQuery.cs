using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class GetAllCursosQuery : IRequest<List<CursoDto>>
    {
    }
}