using AgileStockPro.Web.Models;

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

    public async Task<AppUser?> GetCurrentUserAsync()
    {
        try
        {
            var u = await _api.GetAsync<UserDto>("api/v1/users/me");
            return new AppUser
            {
                BackendIdUsuario = u.IdUsuario,
                BackendIdPersona = u.IdPersona,
                Username = u.Username,
                Name = u.Nombre ?? string.Empty,
                LastName = u.Apellido ?? string.Empty,
                Email = u.Correo ?? string.Empty,
                IsAdmin = string.Equals(u.Rol, "Admin", StringComparison.OrdinalIgnoreCase) || string.Equals(u.Rol, "Administrador", StringComparison.OrdinalIgnoreCase),
                MustChangePassword = u.CambioContrasenaObligatorio,
                Habilitado = true
            };
        }
        catch
        {
            return null;
        }
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        try
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
        catch (Exception ex)
        {
            // Try to parse ProblemDetails for a clearer message
            var msg = TryGetProblemDetails(ex.Message) ?? "Usuario o contraseña incorrectos.";
            return new LoginResult(false, false, msg);
        }
    }

    public async Task<IReadOnlyList<string>> GetSecurityQuestionsAsync(string username)
    {
        try
        {
            var qs = await _api.GetAsync<IEnumerable<PreguntaSeguridadDto>>($"api/v1/securityquestions/{username}");
            return qs.Select(q => q.Pregunta).ToList();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    private static string? TryGetProblemDetails(string content)
    {
        try
        {
            var doc = System.Text.Json.JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("detail", out var d) && d.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                return d.GetString();
            }
            if (doc.RootElement.TryGetProperty("title", out var t) && t.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                return t.GetString();
            }
        }
        catch { }
        return null;
    }

    public Task LogoutAsync()
    {
        _tokens.SetToken(null);
        // Best-effort backend logout to clear auth cookie
        try { _ = _api.PostAsync("api/v1/auth/logout", new { }); } catch { }
        return Task.CompletedTask;
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            // Ensure we have a token; otherwise we'll get 401
            if (string.IsNullOrWhiteSpace(_tokens.Token))
            {
                // Try to infer current token from cookie restored at TokenProvider ctor
            }

            // Server derives username from JWT now
            await _api.PostAsync("api/v1/password/change", new ChangePasswordRequest { Username = string.Empty, OldPassword = currentPassword, NewPassword = newPassword });
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ResetPasswordResult> ResetPasswordAsync(string username, IReadOnlyDictionary<string, string> answers)
    {
        try
        {
            // Obtener preguntas con IDs desde backend y mapear respuestas por texto a sus IDs
            var qs = await _api.GetAsync<IEnumerable<PreguntaSeguridadDto>>($"api/v1/securityquestions/{username}");
            var byText = qs.ToDictionary(q => q.Pregunta, q => q.IdPregunta, StringComparer.OrdinalIgnoreCase);
            var mapped = new Dictionary<int, string>();
            foreach (var kv in answers)
            {
                if (byText.TryGetValue(kv.Key, out var id))
                {
                    mapped[id] = kv.Value;
                }
            }
            await _api.PostAsync("api/v1/password/recover", new RecoverPasswordRequest { Username = username, Answers = mapped });
            return new ResetPasswordResult(true, null, null);
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            return new ResetPasswordResult(false, null, string.IsNullOrWhiteSpace(msg) ? "No se pudo resetear la contraseña" : msg);
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
