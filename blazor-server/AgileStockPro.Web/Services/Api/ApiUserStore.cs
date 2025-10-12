using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services.Api;

public class ApiUserStore : IUserStore
{
    private readonly BackendApiClient _api;

    public ApiUserStore(BackendApiClient api)
    {
        _api = api;
    }

    public async Task<IReadOnlyList<AppUser>> GetAllAsync()
    {
    var paged = await _api.GetAsync<PagedResponse<UserDto>>("api/v1/users");
        var list = (paged.Items ?? Array.Empty<UserDto>()).Select(Map).ToList();
        return list;
    }

    public async Task<AppUser?> FindByUsernameAsync(string username)
    {
        // Use current user endpoint if asking for current; otherwise filter list (basic for now)
    var me = await _api.GetAsync<UserDto>("api/v1/users/me");
        if (string.Equals(me.Username, username, StringComparison.OrdinalIgnoreCase)) return Map(me);
        var all = await GetAllAsync();
        return all.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(AppUser user)
    {
        if (user.Id == Guid.Empty)
        {
            throw new NotSupportedException("Creación requiere asignar Persona y Rol; implementar UI de asignación");
        }
        // We would need UpdateUserRequest mapping; keeping placeholder for later
        throw new NotSupportedException("Actualizar usuario no implementado aún en Blazor");
    }

    public async Task DeleteAsync(Guid id)
    {
        var all = await GetAllAsync();
        var target = all.FirstOrDefault(u => u.Id == id);
        if (target == null) return;
        // Use backend identifier if available
        if (target.BackendIdUsuario is int idUsuario)
        {
            await _api.DeleteAsync($"api/v1/users/{idUsuario}");
            return;
        }
        // Fallback: resolve by username
        var adminList = await _api.GetAsync<PagedResponse<UserDto>>("api/v1/users");
        var match = adminList.Items?.FirstOrDefault(u => u.Username.Equals(target.Username, StringComparison.OrdinalIgnoreCase));
        if (match == null) return;
        await _api.DeleteAsync($"api/v1/users/{match.IdUsuario}");
    }

    public async Task<SecurityPolicy> GetPolicyAsync()
    {
        var dto = await _api.GetAsync<PoliticaSeguridadDto>("api/v1/securitypolicy");
        return new SecurityPolicy
        {
            MinLength = dto.MinCaracteres,
            QuestionsCount = dto.CantPreguntas,
            RequireUpperLower = dto.MayusYMinus,
            RequireNumber = dto.LetrasYNumeros,
            RequireSpecial = dto.CaracterEspecial,
            Require2FA = dto.Autenticacion2FA,
            PreventReuse = dto.NoRepetirAnteriores,
            CheckPersonalData = dto.SinDatosPersonales,
            LockoutMinutes = 5,
            MaxFailedAttempts = 5
        };
    }

    public Task SavePolicyAsync(SecurityPolicy policy)
    {
        // Map back to backend
        return Task.CompletedTask;
    }

    private static AppUser Map(UserDto u) => new AppUser
    {
        Id = Guid.NewGuid(), // UI id only
        BackendIdUsuario = u.IdUsuario,
        BackendIdPersona = u.IdPersona,
        Username = u.Username,
        Name = u.Nombre ?? string.Empty,
        LastName = u.Apellido ?? string.Empty,
        Email = u.Correo ?? string.Empty,
        IsAdmin = string.Equals(u.Rol, "Admin", StringComparison.OrdinalIgnoreCase) || string.Equals(u.Rol, "Administrador", StringComparison.OrdinalIgnoreCase),
        MustChangePassword = u.CambioContrasenaObligatorio
    };

    public Task SetCurrentUserAsync(AppUser? user) => Task.CompletedTask;
    public Task<AppUser?> GetCurrentUserAsync() => Task.FromResult<AppUser?>(null);
}
