using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Session;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthenticationService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        // src/Services/Controllers/AuthController.cs (assumed snippet around line 36)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResult = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (!authResult.Success || authResult.User == null)
                return Unauthorized("Invalid credentials");

            // Si la autenticación es exitosa pero requiere 2FA, no generamos token aún.
            if (authResult.Requires2fa)
            {
                return Ok(new { authResult.Requires2fa });
            }

            var user = authResult.User;
            var token = _tokenService.GenerateJwtToken(user.Username);

            var response = new
            {
                Token = token,
                Username = user.Username,
                Rol = user.Rol ?? "Unknown",
            };
            return Ok(response);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    // src/Services/Controllers/AuthController.cs
    [HttpPost("validate-2fa")]
    public async Task<IActionResult> Validate2fa([FromBody] Validate2faRequest request)
    {
        var authResult = await _authService.Validate2faAsync(request.Username, request.Code);

        if (!authResult.Success || authResult.User == null)
            return Unauthorized("Invalid 2FA code");

        var user = authResult.User;
        var token = _tokenService.GenerateJwtToken(user.Username);

        var response = new
        {
            Token = token,
            Username = user.Username,
            Rol = user.Rol ?? "Unknown",
        };
        return Ok(response);
    }

    public class Validate2faRequest
    {
        public string Username { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}