// src/BusinessLogic/Services/IEmailService.cs
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string newPassword);
        Task Send2faCodeEmailAsync(string toEmail, string code);
        Task SendWelcomeEmailAsync(string toEmail, string username, string password);
    }
}
