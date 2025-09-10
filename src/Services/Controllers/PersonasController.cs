using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Services.Models;
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
        public async Task<IActionResult> Get()
        {
            var personas = await _personaService.GetPersonasAsync();
            return Ok(ApiResponse<List<PersonaDto>>.CreateSuccess(personas));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var persona = await _personaService.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                return NotFound(ApiResponse<object>.CreateFailure("PersonaNotFound", $"Persona with ID {id} not found."));
            }
            return Ok(ApiResponse<PersonaDto>.CreateSuccess(persona));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            var response = ApiResponse<PersonaDto>.CreateSuccess(newPersona);
            return CreatedAtAction(nameof(Get), new { id = newPersona.IdPersona }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaDto personaDto)
        {
            if (id != personaDto.IdPersona)
            {
                return BadRequest(ApiResponse<object>.CreateFailure("IdMismatch", "The ID in the URL must match the ID in the request body."));
            }

            var updatedPersona = await _personaService.UpdatePersonaAsync(personaDto);
            if (updatedPersona == null)
            {
                return NotFound(ApiResponse<object>.CreateFailure("PersonaNotFound", $"Persona with ID {id} not found."));
            }

            return Ok(ApiResponse<PersonaDto>.CreateSuccess(updatedPersona));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return Ok(ApiResponse<object>.CreateSuccess(new { message = $"Persona with ID {id} deleted successfully." }));
        }
    }
}
