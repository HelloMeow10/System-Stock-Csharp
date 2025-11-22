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

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var result = await _authService.LoginAsync(username, password);
            if (!result.Success || result.Requires2FA)
            {
                // Requiere 2FA o error -> no autenticamos aún.
                return false;
            }
            var user = await _authService.GetCurrentUserAsync();
            var claims = new List<Claim>();
            // Nombre
            if (!string.IsNullOrWhiteSpace(user?.Username))
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
            else
                claims.Add(new Claim(ClaimTypes.Name, username));
            // Rol
            if (user?.IsAdmin == true)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            // PersonaId
            if (user?.BackendIdPersona is int pid)
                claims.Add(new Claim("PersonaId", pid.ToString()));
            // Cambio obligatorio
            if (user?.MustChangePassword == true)
                claims.Add(new Claim("ForcePasswordChange", "true"));
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
