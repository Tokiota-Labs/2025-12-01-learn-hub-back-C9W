using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnHub.Back.Application.Handlers.Inscripcion;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InscripcionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<InscripcionDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllInscripcionesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InscripcionDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetInscripcionByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateInscripcionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateInscripcionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteInscripcionCommand { Id = id });
            return NoContent();
        }
    }
}