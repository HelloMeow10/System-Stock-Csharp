using Contracts;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ISecurityPolicyService
    {
        Task<PoliticaSeguridadDto> GetPoliticaSeguridadAsync();
        Task UpdatePoliticaSeguridadAsync(UpdatePoliticaSeguridadRequest request);
    }
}
