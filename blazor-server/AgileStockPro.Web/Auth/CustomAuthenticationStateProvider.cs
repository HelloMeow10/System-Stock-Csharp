using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using AgileStockPro.Web.Services;

namespace AgileStockPro.Web.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Build principal from backend user (via token) so state survives reloads
            var principal = await BuildPrincipalAsync();
            _currentUser = principal;
            return new AuthenticationState(_currentUser);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var result = await _authService.LoginAsync(username, password);
            if (!result.Success || result.Requires2FA)
            {
                // Requiere 2FA o error -> no autenticamos aún.
                return false;
            }
            await RefreshAsync();
            return true;
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task RefreshAsync()
        {
            _currentUser = await BuildPrincipalAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        private async Task<ClaimsPrincipal> BuildPrincipalAsync()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }
                var claims = new List<Claim>();
                if (!string.IsNullOrWhiteSpace(user.Username))
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                claims.Add(new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"));
                if (user.BackendIdPersona is int pid)
                    claims.Add(new Claim("PersonaId", pid.ToString()));
                if (user.MustChangePassword == true)
                    claims.Add(new Claim("ForcePasswordChange", "true"));
                var identity = new ClaimsIdentity(claims, "CustomAuth");
                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }
    }
}
