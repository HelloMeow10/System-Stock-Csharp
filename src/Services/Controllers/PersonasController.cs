using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonaDto>>> Get()
        {
            var personas = await _personaService.GetPersonasAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDto>> Get(int id)
        {
            var persona = await _personaService.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult<PersonaDto>> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            return CreatedAtAction(nameof(Get), new { id = newPersona.IdPersona }, newPersona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaDto personaDto)
        {
            if (id != personaDto.IdPersona)
            {
                return BadRequest();
            }
            await _personaService.UpdatePersonaAsync(personaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }
    }
}
