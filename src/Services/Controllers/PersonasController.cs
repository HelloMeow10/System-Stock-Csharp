using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Services.Hateoas;
using BusinessLogic.Exceptions;
using BusinessLogic.Mappers;

namespace Services.Controllers
{
    public class PersonasController : BaseApiController
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet(Name = "GetPersonas")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResponse<PersonaDto>), StatusCodes.Status200OK)]
        public async Task<PagedResponse<PersonaDto>> Get([FromQuery] PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaService.GetPersonasAsync(paginationParams);
            return new PagedResponse<PersonaDto>(pagedPersonas.Items, pagedPersonas.CurrentPage, pagedPersonas.PageSize, pagedPersonas.TotalCount);
        }

        [HttpGet("{id}", Name = "GetPersonaById")]
        [Authorize]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<PersonaDto> Get(int id)
        {
            return await _personaService.GetPersonaByIdAsync(id);
        }

        [HttpPost(Name = "CreatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaDto>> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            return CreatedAtRoute("GetPersonaById", new { id = newPersona.IdPersona }, newPersona);
        }

        [HttpPut("{id}", Name = "UpdatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePersonaRequest personaDto)
        {
            await _personaService.UpdatePersonaAsync(id, personaDto);
            return NoContent();
        }

        [HttpPatch("{id}", Name = "PatchPersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            await _personaService.PatchPersonaAsync(id, patchDoc);
            return NoContent();
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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }
    }
}
