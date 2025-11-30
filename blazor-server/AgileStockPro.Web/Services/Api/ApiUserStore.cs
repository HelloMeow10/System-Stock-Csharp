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
        if (user.BackendIdUsuario is not int idUsuario)
        {
            // Create
            if (user.BackendIdPersona is not int idPersona) throw new InvalidOperationException("Persona requerida");
            if (user.BackendIdRol is not int idRol && !user.IsAdmin) throw new InvalidOperationException("Rol requerido");
            var rolNombre = user.IsAdmin ? "Admin" : null; // si no admin, debería definirse según selección UI
            var tempPass = string.IsNullOrWhiteSpace(user.TempPassword) ? GenerateTemporaryPassword() : user.TempPassword!;
            var created = await _api.PostAsync<UserDto>("api/v1/users", new UserCreateRequest
            {
                PersonaId = idPersona.ToString(),
                Username = user.Username,
                Password = tempPass,
                Rol = rolNombre ?? "Usuario"
            });
            user.BackendIdUsuario = created.IdUsuario;
            user.BackendIdPersona = created.IdPersona;
            user.MustChangePassword = true;
            user.TempPassword = tempPass;
            return;
        }
        else
        {
            // Update
            var req = new UpdateUserRequest
            {
                Nombre = user.Name,
                Apellido = user.LastName,
                Correo = user.Email,
                IdRol = user.BackendIdRol ?? (user.IsAdmin ? 1 : 2),
                CambioContrasenaObligatorio = user.MustChangePassword,
                FechaExpiracion = user.Expira,
                Habilitado = user.Habilitado
            };
            await _api.PutAsync<UserDto>($"api/v1/users/{idUsuario}", req);
        }
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
        var dto = await _api.GetAsync<PoliticaSeguridadDto>($"api/v1/securitypolicy?ts={Uri.EscapeDataString(DateTime.UtcNow.Ticks.ToString())}");
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

    public async Task SavePolicyAsync(SecurityPolicy policy)
    {
        // Map back to backend request
        var req = new UpdatePoliticaSeguridadRequest
        {
            MinCaracteres = policy.MinLength,
            CantPreguntas = policy.QuestionsCount,
            MayusYMinus = policy.RequireUpperLower,
            LetrasYNumeros = policy.RequireNumber,
            CaracterEspecial = policy.RequireSpecial,
            Autenticacion2FA = policy.Require2FA,
            NoRepetirAnteriores = policy.PreventReuse,
            SinDatosPersonales = policy.CheckPersonalData
        };
        await _api.PutAsync<PoliticaSeguridadDto>("api/v1/securitypolicy", req);
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
        MustChangePassword = u.CambioContrasenaObligatorio,
        Habilitado = u.Habilitado,
        Expira = u.FechaExpiracion
    };

    public Task SetCurrentUserAsync(AppUser? user) => Task.CompletedTask;
    public async Task<AppUser?> GetCurrentUserAsync()
    {
        try
        {
            var u = await _api.GetAsync<UserDto>("api/v1/users/me");
            return Map(u);
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestionsAsync(string username)
    {
        try
        {
            var dtos = await _api.GetAsync<IEnumerable<PreguntaSeguridadDto>>($"api/v1/securityquestions/{username}");
            if (dtos == null) return Enumerable.Empty<SecurityQuestion>();
            return dtos.Select(d => new SecurityQuestion { Question = d.Pregunta, AnswerHash = "HIDDEN" });
        }
        catch
        {
            return Enumerable.Empty<SecurityQuestion>();
        }
    }

    private static string GenerateTemporaryPassword()
    {
        // Simple strong password generator: Upper, lower, digit, special + length 12
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string special = "@$!%*?&";
        var rnd = new Random();
        string Take(string s) => s[rnd.Next(s.Length)].ToString();
        var chars = new List<char>
        {
            Take(upper)[0], Take(lower)[0], Take(digits)[0], Take(special)[0]
        };
        string pool = upper + lower + digits + special;
        while (chars.Count < 12) chars.Add(pool[rnd.Next(pool.Length)]);
        return new string(chars.OrderBy(_ => rnd.Next()).ToArray());
    }
}
