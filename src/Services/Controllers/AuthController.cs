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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResult = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (!authResult.Success || authResult.User == null)
            {
                throw new AuthenticationException("Invalid username or password.");
            }

            if (authResult.Requires2fa)
            {
                return Ok(new LoginResponse { Requires2fa = true });
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
            return Ok(response);
        }

        [HttpPost("validate-2fa")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Validate2fa([FromBody] Validate2faRequest request)
        {
            var authResult = await _authService.Validate2faAsync(request.Username, request.Code);

            if (!authResult.Success || authResult.User == null)
            {
                throw new AuthenticationException("Invalid 2FA code.");
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
            return Ok(response);
        }
    }
}