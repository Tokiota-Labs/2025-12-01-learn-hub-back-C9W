using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnHub.Back.Application.Handlers.Curso;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CursoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCursosQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCursoByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCursoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCursoCommand command)
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
            await _mediator.Send(new DeleteCursoCommand { Id = id });
            return NoContent();
        }
    }
}