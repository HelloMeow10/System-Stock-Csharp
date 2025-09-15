using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers
{
    public class PasswordController : BaseApiController
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("change")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await _passwordService.ChangePasswordAsync(request.Username, request.NewPassword, request.OldPassword);
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPost("recover")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            await _passwordService.RecoverPasswordAsync(request.Username, request.Answers);
            return Ok(new { message = "If the user exists, a password recovery email has been sent." });
        }
    }
}
