using System.Collections.Generic;
using System.Threading.Tasks;
using SharedKernel;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IPersonaRepository
    {
        Task<Persona?> GetPersonaByIdAsync(int id);
        Task<PagedList<Persona>> GetPersonasAsync(PaginationParams paginationParams);
        Task AddPersonaAsync(Persona persona);
        Task UpdatePersonaAsync(Persona persona);
        Task DeletePersonaAsync(int personaId);
    }
}
