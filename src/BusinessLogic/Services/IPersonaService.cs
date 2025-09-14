using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using SharedKernel;

namespace BusinessLogic.Services
{
    public interface IPersonaService
    {
        Task<PersonaDto> CreatePersonaAsync(PersonaRequest request);
        Task<PersonaDto> UpdatePersonaAsync(int id, UpdatePersonaRequest request);
        Task DeletePersonaAsync(int personaId);
        Task<PagedList<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams);
        Task<PersonaDto?> GetPersonaByIdAsync(int personaId);
        Task<UpdatePersonaRequest?> GetPersonaForUpdateAsync(int id);
    }
}
