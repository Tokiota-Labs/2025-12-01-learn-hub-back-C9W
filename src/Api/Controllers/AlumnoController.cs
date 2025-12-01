using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnHub.Back.Application.Handlers.Alumno;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlumnoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumnoDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllAlumnosQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetAlumnoByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAlumnoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateAlumnoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAlumnoCommand { Id = id });
            return NoContent();
        }
    }
}