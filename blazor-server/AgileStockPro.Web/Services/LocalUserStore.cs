using System.Text.Json;
using AgileStockPro.Web.Models;
using Microsoft.JSInterop;

namespace AgileStockPro.Web.Services;

public class LocalUserStore : IUserStore
{
    private const string UsersKey = "users";
    private const string PolicyKey = "user_policy";
    private const string CurrentKey = "current_user";
    private readonly IJSRuntime _js;

    public LocalUserStore(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<IReadOnlyList<AppUser>> GetAllAsync()
    {
        var json = await _js.InvokeAsync<string?>("app.getLocal", UsersKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            // seed admin user
            var admin = new AppUser
            {
                Username = "admin",
                Name = "Admin",
                LastName = "",
                Email = "admin@example.com",
                IsAdmin = true,
                MustChangePassword = true,
            };
            var passHash = await _js.InvokeAsync<string>("app.sha256", admin.Username + "Admin123!");
            admin.PasswordHash = passHash;
            admin.SecurityQuestions = new()
            {
                new SecurityQuestion{ Question = SecurityQuestionsCatalog.Default[0], AnswerHash = await _js.InvokeAsync<string>("app.sha256", "respuesta") },
                new SecurityQuestion{ Question = SecurityQuestionsCatalog.Default[1], AnswerHash = await _js.InvokeAsync<string>("app.sha256", "respuesta") },
            };
            await SaveAsync(admin);
            return new List<AppUser> { admin };
        }
        return JsonSerializer.Deserialize<List<AppUser>>(json!) ?? new List<AppUser>();
    }

    public async Task<AppUser?> FindByUsernameAsync(string username)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(AppUser user)
    {
        var all = (await GetAllAsync()).ToList();
        var idx = all.FindIndex(u => u.Id == user.Id);
        if (idx >= 0) all[idx] = user; else all.Add(user);
        var json = JsonSerializer.Serialize(all);
        await _js.InvokeVoidAsync("app.setLocal", UsersKey, json);
    }

    public async Task DeleteAsync(Guid id)
    {
        var all = (await GetAllAsync()).ToList();
        all.RemoveAll(u => u.Id == id);
        var json = JsonSerializer.Serialize(all);
        await _js.InvokeVoidAsync("app.setLocal", UsersKey, json);
    }

    public async Task<SecurityPolicy> GetPolicyAsync()
    {
        var json = await _js.InvokeAsync<string?>("app.getLocal", PolicyKey);
        if (string.IsNullOrWhiteSpace(json)) return new SecurityPolicy();
        return JsonSerializer.Deserialize<SecurityPolicy>(json!) ?? new SecurityPolicy();
    }

    public async Task SavePolicyAsync(SecurityPolicy policy)
    {
        var json = JsonSerializer.Serialize(policy);
        await _js.InvokeVoidAsync("app.setLocal", PolicyKey, json);
    }

    public async Task SetCurrentUserAsync(AppUser? user)
    {
        if (user == null)
        {
            await _js.InvokeVoidAsync("app.setLocal", CurrentKey, "");
            return;
        }
        var json = JsonSerializer.Serialize(user);
        await _js.InvokeVoidAsync("app.setLocal", CurrentKey, json);
    }

    public async Task<AppUser?> GetCurrentUserAsync()
    {
        var json = await _js.InvokeAsync<string?>("app.getLocal", CurrentKey);
        if (string.IsNullOrWhiteSpace(json)) return null;
        return JsonSerializer.Deserialize<AppUser>(json!);
    }
    public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestionsAsync(string username)
    {
        var user = await FindByUsernameAsync(username);
        return user?.SecurityQuestions ?? Enumerable.Empty<SecurityQuestion>();
    }
}
