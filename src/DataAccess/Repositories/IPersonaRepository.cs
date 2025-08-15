using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IPersonaRepository
    {
        Persona? GetPersonaById(int id);
        List<Persona> GetAllPersonas();
        void AddPersona(Persona persona);
        void UpdatePersona(Persona persona);
        void DeletePersona(int personaId);
    }
}
