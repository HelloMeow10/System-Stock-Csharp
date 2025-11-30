using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public interface IUserStore
{
    Task<IReadOnlyList<AppUser>> GetAllAsync();
    Task<AppUser?> FindByUsernameAsync(string username);
    Task SaveAsync(AppUser user);
    Task DeleteAsync(Guid id);
    Task<SecurityPolicy> GetPolicyAsync();
    Task SavePolicyAsync(SecurityPolicy policy);
    Task SetCurrentUserAsync(AppUser? user);
    Task<AppUser?> GetCurrentUserAsync();
    Task<IEnumerable<SecurityQuestion>> GetSecurityQuestionsAsync(string username);
}
