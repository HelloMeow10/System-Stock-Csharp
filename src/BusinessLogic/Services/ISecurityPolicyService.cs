using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface ISecurityPolicyService
    {
        PoliticaSeguridadDto? GetPoliticaSeguridad();
        PoliticaSeguridadDto UpdatePoliticaSeguridad(PoliticaSeguridadDto politica);
    }
}
