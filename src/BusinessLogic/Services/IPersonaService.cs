using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using SharedKernel;

namespace BusinessLogic.Services
{
    public interface IPersonaService
    {
        Task<PersonaDto> CreatePersonaAsync(PersonaRequest request);
        Task<PersonaDto> UpdatePersonaAsync(int id, PersonaDto personaDto);
        Task<PersonaDto> UpdatePersonaAsync(int id, JsonPatchDocument<UpdatePersonaRequest> patchDoc);
        Task DeletePersonaAsync(int personaId);
        Task<PagedList<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams);
        Task<PersonaDto?> GetPersonaByIdAsync(int personaId);
    }
}
