using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface IPersonaService
    {
        Task<PersonaDto> CreatePersonaAsync(PersonaRequest request);
        Task UpdatePersonaAsync(PersonaDto persona);
        Task DeletePersonaAsync(int personaId);
        Task<List<PersonaDto>> GetPersonasAsync();
        Task<PersonaDto?> GetPersonaByIdAsync(int personaId);
    }
}
