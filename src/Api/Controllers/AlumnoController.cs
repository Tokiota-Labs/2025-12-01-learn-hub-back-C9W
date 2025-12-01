using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Alumno;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Gestión de alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlumnoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los alumnos registrados
        /// </summary>
        /// <returns>Lista de todos los alumnos</returns>
        /// <response code="200">Retorna la lista de alumnos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtiene todos los alumnos",
            Description = "Recupera una lista completa de todos los alumnos registrados en el sistema")]
        [ProducesResponseType(typeof(List<AlumnoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AlumnoDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllAlumnosQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un alumno por su ID
        /// </summary>
        /// <param name="id">ID único del alumno</param>
        /// <returns>Datos del alumno solicitado</returns>
        /// <response code="200">Retorna el alumno solicitado</response>
        /// <response code="404">No se encontró el alumno</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtiene un alumno específico",
            Description = "Recupera los datos de un alumno específico basado en su ID")]
        [ProducesResponseType(typeof(AlumnoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AlumnoDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAlumnoByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo alumno
        /// </summary>
        /// <param name="command">Datos del alumno a crear</param>
        /// <returns>Alumno creado</returns>
        /// <response code="201">Alumno creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registra un nuevo alumno",
            Description = "Crea un nuevo registro de alumno en el sistema")]
        [ProducesResponseType(typeof(AlumnoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateAlumnoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Actualiza los datos de un alumno existente
        /// </summary>
        /// <param name="id">ID del alumno a actualizar</param>
        /// <param name="command">Nuevos datos del alumno</param>
        /// <response code="204">Actualización exitosa</response>
        /// <response code="400">ID no coincide con el objeto o datos inválidos</response>
        /// <response code="404">Alumno no encontrado</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualiza un alumno existente",
            Description = "Actualiza los datos de un alumno específico")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UpdateAlumnoCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Elimina un alumno del sistema
        /// </summary>
        /// <param name="id">ID del alumno a eliminar</param>
        /// <response code="204">Eliminación exitosa</response>
        /// <response code="404">Alumno no encontrado</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Elimina un alumno",
            Description = "Elimina permanentemente un alumno del sistema")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteAlumnoCommand { Id = id });
            return NoContent();
        }
    }
}