using System.Threading.Tasks;
using Contracts;

namespace BusinessLogic.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(string username, string password);
        Task<AuthenticationResult> Validate2faAsync(string username, string code);
    }
}
