using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Services.Hateoas;
using System.Linq;

namespace Services.Controllers
{
    [Authorize]
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
        /// <param name="paginationParams">The pagination parameters (pageNumber, pageSize).</param>
        /// <returns>A paginated list of personas with HATEOAS links.</returns>
        /// <response code="200">Returns the paginated list of personas.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet(Name = "GetPersonas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PersonaDto>>> Get([FromQuery] PaginationParams paginationParams)
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

            var links = _linkService.CreateLinksForCollection(Url, pagedPersonas, "GetPersonas", paginationParams);
            var linkedPersonas = pagedPersonas.Items.Select(persona => {
                _linkService.AddLinksForPersona(Url, persona);
                return persona;
            });

            var result = new
            {
                value = linkedPersonas,
                links
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific persona by their ID.
        /// </summary>
        /// <param name="id">The ID of the persona to retrieve.</param>
        /// <returns>The requested persona with HATEOAS links.</returns>
        /// <response code="200">Returns the requested persona.</response>
        /// <response code="404">If the persona is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("{id}", Name = "GetPersonaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PersonaDto>> Get(int id)
        {
            var persona = await _personaService.GetPersonaByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            _linkService.AddLinksForPersona(Url, persona);

            return Ok(persona);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);

            _linkService.AddLinksForPersona(Url, newPersona);

            return CreatedAtRoute("GetPersonaById", new { id = newPersona.IdPersona }, newPersona);
        }

        [HttpPut("{id}", Name = "UpdatePersona")]
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

            _linkService.AddLinksForPersona(Url, updatedPersona);

            return Ok(updatedPersona);
        }

        [HttpDelete("{id}", Name = "DeletePersona")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeletePersonaAsync(id);
            return NoContent();
        }
    }
}
