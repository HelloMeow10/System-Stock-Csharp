using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Services.Hateoas;
using System.Linq;

namespace Services.Controllers
{
    public class PersonasController : BaseApiController
    {
        private readonly IPersonaService _personaService;
        private readonly ILinkService _linkService;

        public PersonasController(IPersonaService personaService, ILinkService linkService)
        {
            _personaService = personaService;
            _linkService = linkService;
        }

        [HttpGet(Name = "GetPersonas")]
        [Authorize]
        [ProducesResponseType(typeof(PagedApiResponse<IEnumerable<PersonaDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaService.GetPersonasAsync(paginationParams);

            var response = new PagedApiResponse<IEnumerable<PersonaDto>>(pagedPersonas.Items, pagedPersonas.CurrentPage, pagedPersonas.PageSize, pagedPersonas.TotalCount);

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetPersonaById")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var persona = await _personaService.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                return NotFound(ApiResponse<PersonaDto>.Fail("Persona not found."));
            }

            return Ok(ApiResponse<PersonaDto>.Success(persona));
        }

        [HttpPost(Name = "CreatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);

            var response = ApiResponse<PersonaDto>.Success(newPersona);

            return CreatedAtRoute("GetPersonaById", new { id = newPersona.IdPersona }, response);
        }

        [HttpPut("{id}", Name = "UpdatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaDto personaDto)
        {
            var updatedPersona = await _personaService.UpdatePersonaAsync(id, personaDto);
            if (updatedPersona == null)
            {
                return NotFound(ApiResponse<PersonaDto>.Fail("Persona not found."));
            }
            return Ok(ApiResponse<PersonaDto>.Success(updatedPersona));
        }

        [HttpPatch("{id}", Name = "PatchPersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PersonaDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(ApiResponse<PersonaDto>.Fail("A patch document is required."));
            }

            var updatedPersona = await _personaService.UpdatePersonaAsync(id, patchDoc);
            if (updatedPersona == null)
            {
                return NotFound(ApiResponse<PersonaDto>.Fail("Persona not found."));
            }

            return Ok(ApiResponse<PersonaDto>.Success(updatedPersona));
        }

        /// <summary>
        /// Deletes a persona.
        /// </summary>
        /// <param name="id">The ID of the persona to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">The persona was successfully deleted.</response>
        /// <response code="404">If the persona is not found.</response>
        [HttpDelete("{id}", Name = "DeletePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }
    }
}
