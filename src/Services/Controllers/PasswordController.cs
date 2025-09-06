using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await _passwordService.ChangePasswordAsync(request.Username, request.NewPassword, request.OldPassword);
            return NoContent();
        }

        [HttpPost("recover")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            await _passwordService.RecoverPasswordAsync(request.Username, request.Answers);
            return Ok();
        }
    }
}
