using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Alumno
{
    public class GetAllAlumnosQuery : IRequest<List<AlumnoDto>>
    {
    }
}