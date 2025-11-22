using AgileStockPro.Web.Models;
namespace AgileStockPro.Web.Services;

public class UserStoreInMemory : IUserStore
{
    private readonly List<AppUser> _users = new();
    private AppUser? _current;
    private SecurityPolicy _policy = new SecurityPolicy
    {
        MinLength = 8,
        RequireUpperLower = true,
        RequireNumber = true,
        RequireSpecial = false,
        PreventReuse = true,
        LockoutMinutes = 5,
        MaxFailedAttempts = 5,
        CheckPersonalData = false,
        Require2FA = false,
        QuestionsCount = 0
    };

    public UserStoreInMemory()
    {
        // Seed default admin user so initial login works without backend integration.
        // Password: admin123 (hash = SHA256("admin" + "admin123"))
        const string adminPasswordHash = "1eb1afa20dc454d6ef3b6dc6abcbd7dca7e519b698fdf073f4625ded09d74807";
        if (!_users.Any(u => u.Username == "admin"))
        {
            _users.Add(new AppUser
            {
                Username = "admin",
                Name = "Admin",
                LastName = "",
                Email = "admin@example.com",
                IsAdmin = true,
                PasswordHash = adminPasswordHash,
                MustChangePassword = false,
                SecurityQuestions = new List<SecurityQuestion>
                {
                    new SecurityQuestion { Question = "¿Cuál fue el nombre de tu primera mascota?", AnswerHash = adminPasswordHash },
                    new SecurityQuestion { Question = "¿En qué ciudad naciste?", AnswerHash = adminPasswordHash }
                },
                PasswordHistory = new List<PasswordHistoryEntry>()
            });
        }
    }

    public Task<IReadOnlyList<AppUser>> GetAllAsync() => Task.FromResult<IReadOnlyList<AppUser>>(_users);
    public Task<AppUser?> FindByUsernameAsync(string username) => Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
    public Task SaveAsync(AppUser user)
    {
        var existing = _users.FindIndex(u => u.Id == user.Id);
        if (existing >= 0) _users[existing] = user; else _users.Add(user);
        return Task.CompletedTask;
    }
    public Task DeleteAsync(Guid id)
    {
        _users.RemoveAll(u => u.Id == id);
        return Task.CompletedTask;
    }
    public Task<SecurityPolicy> GetPolicyAsync() => Task.FromResult(_policy);
    public Task SavePolicyAsync(SecurityPolicy policy)
    {
        _policy = policy; return Task.CompletedTask;
    }
    public Task SetCurrentUserAsync(AppUser? user)
    { _current = user; return Task.CompletedTask; }
    public Task<AppUser?> GetCurrentUserAsync() => Task.FromResult(_current);
}