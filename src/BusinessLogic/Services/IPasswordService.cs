using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IPasswordService
    {
        Task RecoverPasswordAsync(string username, Dictionary<int, string> anwers);
        Task ChangePasswordAsync(string username, string newPassword, string oldPassword);
    }
}
