using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Services.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PasswordController : BaseApiController
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("change", Name = "ChangePasswordV1")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var username = User?.Identity?.Name ?? throw new UnauthorizedAccessException("No user identity available.");
            await _passwordService.ChangePasswordAsync(username, request.NewPassword, request.OldPassword);
            return NoContent();
        }

        /// <summary>
        /// Initiates the password recovery process for a user.
        /// </summary>
        /// <remarks>
        /// If the user exists and the answers are correct, a password recovery email will be sent.
        /// For security reasons, this endpoint will return a successful response even if the user does not exist or the answers are incorrect.
        /// </remarks>
        [HttpPost("recover", Name = "RecoverPasswordV1")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            await _passwordService.RecoverPasswordAsync(request.Username, request.Answers);
            return NoContent();
        }
    }
}
