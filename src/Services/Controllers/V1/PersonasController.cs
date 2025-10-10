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
using Asp.Versioning;
using AutoMapper;

namespace Services.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PersonasController : BaseApiController
    {
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;

        public PersonasController(IPersonaService personaService, IMapper mapper)
        {
            _personaService = personaService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetPersonasV1")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResponse<PersonaDto>), StatusCodes.Status200OK)]
        public Task<PagedResponse<PersonaDto>> Get([FromQuery] PaginationParams paginationParams)
        {
            return _personaService.GetPersonasAsync(paginationParams);
        }

        [HttpGet("{id}", Name = "GetPersonaByIdV1")]
        [Authorize]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<PersonaDto> Get(int id)
        {
            return _personaService.GetPersonaByIdAsync(id);
        }

        [HttpPost(Name = "CreatePersonaV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaDto>> Post([FromBody] PersonaRequest personaRequest)
        {
            var newPersona = await _personaService.CreatePersonaAsync(personaRequest);
            return CreatedAtAction(nameof(Get), new { id = newPersona.IdPersona, version = "1.0" }, newPersona);
        }

        [HttpPut("{id}", Name = "UpdatePersonaV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<PersonaDto> Put(int id, [FromBody] UpdatePersonaRequest personaDto)
        {
            return _personaService.UpdatePersonaAsync(id, personaDto);
        }

        [HttpPatch("{id}", Name = "PatchPersonaV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<PersonaDto> Patch(int id, [FromBody] JsonPatchDocument<UpdatePersonaRequest> patchDoc)
        {
            return _personaService.PatchPersonaAsync(id, patchDoc);
        }

        /// <summary>
        /// Deletes a persona.
        /// </summary>
        /// <param name="id">The ID of the persona to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">The persona was successfully deleted.</response>
        /// <response code="404">If the persona is not found.</response>
        [HttpDelete("{id}", Name = "DeletePersonaV1")]
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
