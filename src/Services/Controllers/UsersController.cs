using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(ApiResponse<List<UserDto>>.CreateSuccess(users));
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(ApiResponse<object>.CreateFailure("UserNotFound", $"User with username '{username}' not found."));
            }
            return Ok(ApiResponse<UserDto>.CreateSuccess(user));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            var response = ApiResponse<UserDto>.CreateSuccess(newUser);
            return CreatedAtAction(nameof(GetByUsername), new { username = newUser.Username }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.IdUsuario)
            {
                return BadRequest(ApiResponse<object>.CreateFailure("IdMismatch", "The ID in the URL must match the ID in the request body."));
            }

            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null)
            {
                return NotFound(ApiResponse<object>.CreateFailure("UserNotFound", $"User with ID {id} not found."));
            }

            return Ok(ApiResponse<UserDto>.CreateSuccess(updatedUser));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(ApiResponse<object>.CreateSuccess(new { message = $"User with ID {id} deleted successfully." }));
        }
    }
}