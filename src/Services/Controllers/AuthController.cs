using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Session;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Http; // Added for CookieOptions
using Microsoft.AspNetCore.Antiforgery; // Added for CSRF

namespace Services.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        private readonly IAntiforgery _antiforgery;
        private const string AuthCookieName = "ums_auth";

        public AuthController(IAuthenticationService authService, ITokenService tokenService, IAntiforgery antiforgery)
        {
            _authService = authService;
            _tokenService = tokenService;
            _antiforgery = antiforgery;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
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
            var token = _tokenService.GenerateJwtToken(user.Username);
            SetAuthCookie(token);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
                Token = token
            });
        }

        [HttpPost("validate-2fa")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
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
            var token = _tokenService.GenerateJwtToken(user.Username);
            SetAuthCookie(token);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
                Token = token
            });
        }

        [HttpGet("csrf-token")]
        [Authorize]
        public IActionResult GetCsrfToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new { token = tokens.RequestToken });
        }

        [Authorize]
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(AuthCookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
            });
            return NoContent();
        }

        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Should be true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) // Example expiration
            };
            Response.Cookies.Append(AuthCookieName, token, cookieOptions);
        }
    }
}