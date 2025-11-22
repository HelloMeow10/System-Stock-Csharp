using System.Security.Claims;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Contracts;

namespace AgileStockPro.Web.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthenticationService _authService;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var result = await _authService.AuthenticateAsync(username, password);

            if (result.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, result.Role ?? "User"),
                    new Claim("UserId", result.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, "CustomAuth");
                _currentUser = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }

            return false;
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
