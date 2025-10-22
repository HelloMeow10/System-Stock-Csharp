using System.Text;
using System.Text.Json;
using AgileStockPro.Web.Models;
using Microsoft.JSInterop;

namespace AgileStockPro.Web.Services;

public class AuthService : IAuthService
{
    private readonly IUserStore _store;
    private readonly IJSRuntime _js;

    public AuthService(IUserStore store, IJSRuntime js)
    {
        _store = store;
        _js = js;
    }

    private async Task<string> Sha256Async(string text)
    {
        return await _js.InvokeAsync<string>("app.sha256", text);
    }

    public Task<AppUser?> GetCurrentUserAsync() => _store.GetCurrentUserAsync();

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var user = await _store.FindByUsernameAsync(username);
        if (user == null) return new LoginResult(false, false, "Usuario o contraseña inválidos");
        var policy = await _store.GetPolicyAsync();
        if (user.LockoutUntil.HasValue && user.LockoutUntil.Value > DateTime.UtcNow)
        {
            var mins = (int)Math.Ceiling((user.LockoutUntil.Value - DateTime.UtcNow).TotalMinutes);
            return new LoginResult(false, false, $"Cuenta bloqueada por {mins} min");
        }
        var composite = username + password;
        var hash = await Sha256Async(composite);
        if (!string.Equals(user.PasswordHash, hash, StringComparison.Ordinal))
        {
            user.FailedAttempts++;
            if (user.FailedAttempts >= policy.MaxFailedAttempts)
            {
                user.LockoutUntil = DateTime.UtcNow.AddMinutes(policy.LockoutMinutes);
                user.FailedAttempts = 0;
            }
            await _store.SaveAsync(user);
            return new LoginResult(false, false, "Usuario o contraseña inválidos");
        }
        user.FailedAttempts = 0;
        user.LockoutUntil = null;
        await _store.SaveAsync(user);
        if (policy.Require2FA)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            await _js.InvokeVoidAsync("app.setLocal", "_2fa_code", code);
            await _store.SetCurrentUserAsync(user);
            return new LoginResult(true, true, null);
        }
        await _store.SetCurrentUserAsync(user);
        return new LoginResult(true, false, null);
    }

    public async Task LogoutAsync() => await _store.SetCurrentUserAsync(null);

    public async Task<(bool Success, string? ErrorMessage)> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var user = await _store.GetCurrentUserAsync();
        if (user == null) return (false, "Usuario no encontrado");
        var currentHash = await Sha256Async(user.Username + currentPassword);
        if (!string.Equals(currentHash, user.PasswordHash, StringComparison.Ordinal))
            return (false, "La contraseña actual es incorrecta.");

        var policy = await _store.GetPolicyAsync();
        if (!ValidatePassword(newPassword, user, policy))
            return (false, "La nueva contraseña no cumple la política de seguridad.");

        var newHash = await Sha256Async(user.Username + newPassword);
        if (policy.PreventReuse && user.PasswordHistory.Any(h => h.Hash == newHash))
            return (false, "No puedes reutilizar una contraseña anterior.");

        user.PasswordHistory.Add(new PasswordHistoryEntry { Hash = user.PasswordHash, ChangedAt = DateTime.UtcNow });
        user.PasswordHash = newHash;
        user.MustChangePassword = false;
        await _store.SaveAsync(user);
        await _store.SetCurrentUserAsync(user);
        return (true, null);
    }

    public async Task<ResetPasswordResult> ResetPasswordAsync(string username, IReadOnlyDictionary<string, string> answers)
    {
        var user = await _store.FindByUsernameAsync(username);
        if (user == null) return new ResetPasswordResult(false, null, "Usuario no encontrado");

        // verify security questions
        foreach (var q in user.SecurityQuestions)
        {
            var provided = answers.TryGetValue(q.Question, out var val) ? val ?? string.Empty : string.Empty;
            var providedHash = await Sha256Async(provided);
            if (!string.Equals(providedHash, q.AnswerHash, StringComparison.Ordinal))
                return new ResetPasswordResult(false, null, "Respuestas incorrectas");
        }

        // generate temp password and force change
        var tempPassword = GenerateTempPassword();
        var hash = await Sha256Async(user.Username + tempPassword);
        user.PasswordHistory.Add(new PasswordHistoryEntry { Hash = user.PasswordHash, ChangedAt = DateTime.UtcNow });
        user.PasswordHash = hash;
        user.MustChangePassword = true;
        await _store.SaveAsync(user);
        return new ResetPasswordResult(true, tempPassword, null);
    }

    private static string GenerateTempPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%^&*";
        var rnd = Random.Shared;
        return new string(Enumerable.Range(0, 12).Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
    }

    private static bool ValidatePassword(string password, AppUser user, SecurityPolicy p)
    {
        if (password.Length < p.MinLength) return false;
        if (p.RequireUpperLower && (!(password.Any(char.IsUpper) && password.Any(char.IsLower)))) return false;
        if (p.RequireNumber && !password.Any(char.IsDigit)) return false;
        if (p.RequireSpecial && password.All(ch => char.IsLetterOrDigit(ch))) return false;
        if (p.CheckPersonalData)
        {
            var tokens = new[] { user.Name, user.LastName, user.Birthdate?.ToString("yyyyMMdd") ?? string.Empty }
                .Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t!.ToLowerInvariant());
            var lower = password.ToLowerInvariant();
            if (tokens.Any(t => t.Length >= 3 && lower.Contains(t))) return false;
        }
        return true;
    }
    public async Task<bool> Verify2FAAsync(string code)
    {
        var stored = await _js.InvokeAsync<string?>("app.getLocal", "_2fa_code");
        var ok = !string.IsNullOrWhiteSpace(stored) && stored == code;
        if (ok)
        {
            await _js.InvokeVoidAsync("app.setLocal", "_2fa_code", "");
        }
        return ok;
    }

    public async Task<IReadOnlyList<string>> GetSecurityQuestionsAsync(string username)
    {
        var user = await _store.FindByUsernameAsync(username);
        return user?.SecurityQuestions.Select(q => q.Question).ToList() ?? new List<string>();
    }
}
