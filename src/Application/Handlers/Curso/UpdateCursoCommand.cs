using MediatR;
using LearnHub.Back.Application.DTOs;
using System;

namespace LearnHub.Back.Application.Handlers.Curso
{
    public class UpdateCursoCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}