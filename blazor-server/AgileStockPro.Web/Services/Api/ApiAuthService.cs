using AgileStockPro.Web.Models;
using Contracts;

namespace AgileStockPro.Web.Services.Api;

public class ApiAuthService : IAuthService
{
    private readonly BackendApiClient _api;
    private readonly ITokenProvider _tokens;
    private string? _lastUsername;

    public ApiAuthService(BackendApiClient api, ITokenProvider tokens)
    {
        _api = api;
        _tokens = tokens;
    }

    public Task<AppUser?> GetCurrentUserAsync() => Task.FromResult<AppUser?>(null); // can be wired to api/v1/users/me later

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var res = await _api.PostAsync<LoginResponse>("api/v1/auth/login", new LoginRequest { Username = username, Password = password });
        if (res.Requires2fa)
        {
            _lastUsername = username;
            return new LoginResult(true, true, null);
        }
        _tokens.SetToken(res.Token);
        return new LoginResult(true, false, null);
    }

    public Task LogoutAsync()
    {
        _tokens.SetToken(null);
        return Task.CompletedTask;
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        // Backend requires username; we can call /users/me first if needed in a future iteration
        // For now, assume the backend derives current user from token and ignores Username
        try
        {
            await _api.PostAsync("api/v1/password/change", new ChangePasswordRequest { Username = string.Empty, OldPassword = currentPassword, NewPassword = newPassword });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ResetPasswordResult> ResetPasswordAsync(string username, IReadOnlyDictionary<string, string> answers)
    {
        try
        {
            // Answers mapping to backend uses question ids; placeholder: send empty dict to trigger generic response
            await _api.PostAsync("api/v1/password/recover", new RecoverPasswordRequest { Username = username, Answers = new Dictionary<int, string>() });
            return new ResetPasswordResult(true, null, null);
        }
        catch (Exception ex)
        {
            return new ResetPasswordResult(false, null, ex.Message);
        }
    }

    public async Task<bool> Verify2FAAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(_lastUsername)) return false;
        try
        {
            var res = await _api.PostAsync<LoginResponse>("api/v1/auth/validate-2fa", new Validate2faRequest { Username = _lastUsername, Code = code });
            _tokens.SetToken(res.Token);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
