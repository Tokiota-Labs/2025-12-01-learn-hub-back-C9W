using MediatR;
using LearnHub.Back.Application.DTOs;
using System;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class GetCursoByIdQuery : IRequest<CursoDto>
    {
        public Guid Id { get; set; }
    }
}