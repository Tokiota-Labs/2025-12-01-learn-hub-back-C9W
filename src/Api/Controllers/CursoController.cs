using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Curso;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Gestión de cursos")]
    public class CursoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CursoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los cursos disponibles
        /// </summary>
        /// <returns>Lista de cursos</returns>
        /// <response code="200">Retorna la lista de cursos</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtiene todos los cursos",
            Description = "Recupera una lista completa de todos los cursos disponibles")]
        [ProducesResponseType(typeof(List<CursoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CursoDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCursosQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un curso específico por su ID
        /// </summary>
        /// <param name="id">ID del curso</param>
        /// <returns>Información detallada del curso</returns>
        /// <response code="200">Retorna el curso solicitado</response>
        /// <response code="404">No se encontró el curso</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtiene un curso por ID",
            Description = "Recupera la información detallada de un curso específico")]
        [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CursoDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCursoByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo curso
        /// </summary>
        /// <param name="command">Datos del curso a crear</param>
        /// <returns>Curso creado</returns>
        /// <response code="201">Curso creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Crea un nuevo curso",
            Description = "Registra un nuevo curso en el sistema")]
        [ProducesResponseType(typeof(CursoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateCursoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Actualiza un curso existente
        /// </summary>
        /// <param name="id">ID del curso a actualizar</param>
        /// <param name="command">Nuevos datos del curso</param>
        /// <response code="204">Actualización exitosa</response>
        /// <response code="400">ID no coincide o datos inválidos</response>
        /// <response code="404">Curso no encontrado</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualiza un curso",
            Description = "Actualiza la información de un curso existente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UpdateCursoCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID de la ruta no coincide con el ID del curso");
            
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Elimina un curso
        /// </summary>
        /// <param name="id">ID del curso a eliminar</param>
        /// <response code="204">Eliminación exitosa</response>
        /// <response code="404">Curso no encontrado</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Elimina un curso",
            Description = "Elimina permanentemente un curso del sistema")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCursoCommand { Id = id });
            return NoContent();
        }
    }
}