using Contracts;
using DataAccess.Entities;
using System.Threading.Tasks;

namespace BusinessLogic.Factories
{
    public interface IPersonaFactory
    {
        Task<Persona> CreateAsync(PersonaRequest request);
    }
}
