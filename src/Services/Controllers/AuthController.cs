using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Session;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;

namespace Services.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthenticationService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
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
                return new LoginResponse { Requires2fa = true };
            }

            if (authResult.User == null)
            {
                // This case should ideally not happen if Success is true and Requires2fa is false,
                // but we handle it defensively.
                throw new AuthenticationException("An unexpected error occurred during authentication.");
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
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
                // This case should not happen if 2FA validation is successful.
                throw new AuthenticationException("An unexpected error occurred during 2FA validation.");
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
        }
    }
}