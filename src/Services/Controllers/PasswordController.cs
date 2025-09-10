using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Threading.Tasks;
using Services.Models;
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await _passwordService.ChangePasswordAsync(request.Username, request.NewPassword, request.OldPassword);
            return Ok(ApiResponse<object>.CreateSuccess(new { message = "Password changed successfully." }));
        }

        [HttpPost("recover")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            await _passwordService.RecoverPasswordAsync(request.Username, request.Answers);
            return Ok(ApiResponse<object>.CreateSuccess(new { message = "If the user exists, a password recovery email has been sent." }));
        }
    }
}
