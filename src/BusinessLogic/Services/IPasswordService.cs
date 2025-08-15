using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IPasswordService
    {
        Task RecuperarContrasena(string username, Dictionary<int, string> respuestas);
        Task CambiarContrasenaAsync(string username, string newPassword, string oldPassword);
    }
}
