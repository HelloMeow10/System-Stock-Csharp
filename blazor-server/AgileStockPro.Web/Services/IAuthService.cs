using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public record ResetPasswordResult(bool Success, string? TempPassword, string? Error);
public record LoginResult(bool Success, bool Requires2FA, string? Error);

public interface IAuthService
{
    Task<AppUser?> GetCurrentUserAsync();
    Task<LoginResult> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    Task<ResetPasswordResult> ResetPasswordAsync(string username, IReadOnlyDictionary<string, string> answers);
    Task<bool> Verify2FAAsync(string code);
    Task<IReadOnlyList<string>> GetSecurityQuestionsAsync(string username);
}
