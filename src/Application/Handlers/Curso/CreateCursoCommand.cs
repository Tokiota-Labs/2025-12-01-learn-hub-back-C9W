using MediatR;
using LearnHub.Back.Application.DTOs;
using System;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class CreateCursoCommand : IRequest<CursoDto>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}