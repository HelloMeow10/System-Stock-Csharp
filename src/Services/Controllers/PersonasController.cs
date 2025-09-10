using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers
{
    [Authorize]
    public class PersonasController : BaseApiController
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaDto>>> Get()
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            return CreatedAtAction(nameof(Get), new { id = newPersona.IdPersona }, newPersona);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaDto personaDto)
        {
            if (id != personaDto.IdPersona)
            {
                return BadRequest("The ID in the URL must match the ID in the request body.");
            }

            var updatedPersona = await _personaService.UpdatePersonaAsync(personaDto);
            if (updatedPersona == null)
            {
                return NotFound();
            }

            return Ok(updatedPersona);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }
    }
}
