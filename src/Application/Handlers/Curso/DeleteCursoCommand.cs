using MediatR;
using System;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class DeleteCursoCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}