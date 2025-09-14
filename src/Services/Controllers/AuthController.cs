using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Session;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResult = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (!authResult.Success || authResult.User == null)
            {
                return Unauthorized(ApiResponse<LoginResponse>.Fail("Invalid username or password."));
            }

            if (authResult.Requires2fa)
            {
                var twoFaResponse = new LoginResponse { Requires2fa = true };
                return Ok(ApiResponse<LoginResponse>.Success(twoFaResponse));
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
            return Ok(ApiResponse<LoginResponse>.Success(response));
        }

        [HttpPost("validate-2fa")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Validate2fa([FromBody] Validate2faRequest request)
        {
            var authResult = await _authService.Validate2faAsync(request.Username, request.Code);

            if (!authResult.Success || authResult.User == null)
            {
                return Unauthorized(ApiResponse<LoginResponse>.Fail("Invalid 2FA code."));
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
            return Ok(ApiResponse<LoginResponse>.Success(response));
        }
    }
}