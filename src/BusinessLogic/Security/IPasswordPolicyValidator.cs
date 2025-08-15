using DataAccess.Entities;

namespace BusinessLogic.Security
{
    public interface IPasswordPolicyValidator
    {
        void Validate(string password, string username, Persona persona, PoliticaSeguridad politica);
    }
}
