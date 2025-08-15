using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface ISecurityPolicyService
    {
        PoliticaSeguridadDto? GetPoliticaSeguridad();
        void UpdatePoliticaSeguridad(PoliticaSeguridadDto politica);
    }
}
