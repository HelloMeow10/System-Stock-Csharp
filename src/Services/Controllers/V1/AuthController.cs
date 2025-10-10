using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Http;
using Session;
using System;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Asp.Versioning;
namespace Services.Controllers.V1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        private const string AuthCookieName = "ums_auth";

        public AuthController(IAuthenticationService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("login", Name = "LoginV1")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var authResult = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (!authResult.Success)
            {
                throw new AuthenticationException(authResult.ErrorMessage ?? "Invalid username or password.");
            }

            if (authResult.Requires2fa)
            {
                return Ok(new LoginResponse { Requires2fa = true });
            }

            if (authResult.User == null)
            {
                throw new AuthenticationException("An unexpected error occurred during authentication.");
            }

            var user = authResult.User;
            var normalizedRole = NormalizeRole(user.Rol);
            var token = _tokenService.GenerateJwtToken(user.Username, normalizedRole);
            SetAuthCookie(token);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Rol = normalizedRole ?? "Unknown",
            });
        }

        [HttpPost("validate-2fa", Name = "Validate2faV1")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginResponse>> Validate2fa([FromBody] Validate2faRequest request)
        {
            var authResult = await _authService.Validate2faAsync(request.Username, request.Code);

            if (!authResult.Success)
            {
                throw new AuthenticationException(authResult.ErrorMessage ?? "Invalid 2FA code.");
            }

            if (authResult.User == null)
            {
                throw new AuthenticationException("An unexpected error occurred during 2FA validation.");
            }

            var user = authResult.User;
            var normalizedRole = NormalizeRole(user.Rol);
            var token = _tokenService.GenerateJwtToken(user.Username, normalizedRole);
            SetAuthCookie(token);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Rol = normalizedRole ?? "Unknown",
            });
        }

        [Authorize]
        [HttpPost("logout", Name = "LogoutV1")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(AuthCookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // set false for HTTP in development
                SameSite = SameSiteMode.Strict,
            });
            return NoContent();
        }

        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // false for HTTP during development
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) // Example expiration
            };
            Response.Cookies.Append(AuthCookieName, token, cookieOptions);
        }

        private static string? NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role)) return role;
            // Map Spanish DB roles to API roles used in [Authorize(Roles = "Admin")]
            if (string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return "Admin";
            }
            if (string.Equals(role, "Usuario", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return "User";
            }
            return role; // leave as-is for other roles
        }
    }
}