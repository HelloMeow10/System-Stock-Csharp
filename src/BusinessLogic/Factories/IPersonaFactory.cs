using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic.Factories
{
    public interface IPersonaFactory
    {
        Persona Create(PersonaRequest request);
    }
}
