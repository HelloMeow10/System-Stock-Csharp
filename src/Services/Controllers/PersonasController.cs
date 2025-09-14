using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Services.Hateoas;

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

        /// <summary>
        /// Retrieves a paginated list of personas.
        /// </summary>
        [HttpGet(Name = "GetPersonas")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
        {
            var pagedPersonas = await _personaService.GetPersonasAsync(paginationParams);

            var paginationMetadata = new
            {
                pagedPersonas.TotalCount,
                pagedPersonas.PageSize,
                pagedPersonas.CurrentPage,
                pagedPersonas.TotalPages,
                pagedPersonas.HasNext,
                pagedPersonas.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            pagedPersonas.Items.ForEach(persona => _linkService.AddLinks(persona));

            var result = new LinkedCollectionResource<PersonaDto>(pagedPersonas.Items);
            result.Links.AddRange(_linkService.GetLinksForCollection(pagedPersonas, "GetPersonas", paginationParams));

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific persona by their ID.
        /// </summary>
        [HttpGet("{id}", Name = "GetPersonaById")]
        [Authorize]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonaDto>> Get(int id)
        {
            var persona = await _personaService.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            _linkService.AddLinks(persona);

            return Ok(persona);
        }

        /// <summary>
        /// Creates a new persona.
        /// </summary>
        [HttpPost(Name = "CreatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            _linkService.AddLinks(newPersona);

            return CreatedAtRoute("GetPersonaById", new { id = newPersona.IdPersona }, newPersona);
        }

        /// <summary>
        /// Fully updates an existing persona.
        /// </summary>
        [HttpPut("{id}", Name = "UpdatePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePersonaRequest request)
        {
            var updatedPersona = await _personaService.UpdatePersonaAsync(id, request);
            if (updatedPersona == null)
            {
                return NotFound();
            }
            _linkService.AddLinks(updatedPersona);
            return Ok(updatedPersona);
        }

        /// <summary>
        /// Partially updates an existing persona.
        /// </summary>
        [HttpPatch("{id}", Name = "PatchPersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("A patch document is required.");
            }

            var personaToPatch = await _personaService.GetPersonaForUpdateAsync(id);
            if (personaToPatch == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(personaToPatch, ModelState);

            if (!TryValidateModel(personaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            var updatedPersona = await _personaService.UpdatePersonaAsync(id, personaToPatch);
            _linkService.AddLinks(updatedPersona);

            return Ok(updatedPersona);
        }

        /// <summary>
        /// Deletes a persona.
        /// </summary>
        [HttpDelete("{id}", Name = "DeletePersona")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }

    }
}
