using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
using Services.Hateoas;

namespace Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILinkService _linkService;

        public UsersController(IUserService userService, ILinkService linkService)
        {
            _userService = userService;
            _linkService = linkService;
        }

        /// <summary>
        /// Partially updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="patchDoc">The JSON patch document with the updates.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="400">If the patch document is invalid or the model state is invalid after patching.</response>
        /// <response code="404">If the user to update is not found.</response>
        [HttpPatch("{id}", Name = "PatchUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(ApiResponse<UserDto>.Fail("A patch document is required."));
            }

            var userToPatch = await _userService.GetUserForUpdateAsync(id);
            if (userToPatch == null)
            {
                return NotFound(ApiResponse<UserDto>.Fail("User not found."));
            }

            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return BadRequest(ApiResponse<UserDto>.Fail(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userToPatch);
            _linkService.AddLinks(updatedUser);

            return Ok(ApiResponse<UserDto>.Success(updatedUser));
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters (pageNumber, pageSize).</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
        {
            var pagedUsers = await _userService.GetUsersAsync(paginationParams);
            pagedUsers.Items.ForEach(user => _linkService.AddLinks(user));

            var response = new PagedApiResponse<IEnumerable<UserDto>>(pagedUsers.Items, pagedUsers.CurrentPage, pagedUsers.PageSize, pagedUsers.TotalCount);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The requested user with HATEOAS links.</returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(ApiResponse<UserDto>.Fail("User not found."));
            }

            _linkService.AddLinks(user);

            return Ok(ApiResponse<UserDto>.Success(user));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user data for creation.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost(Name = "CreateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            _linkService.AddLinks(newUser);

            var response = ApiResponse<UserDto>.Success(newUser);

            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario }, response);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserRequest">The user data for the update.</param>
        /// <returns>The updated user.</returns>
        [HttpPut("{id}", Name = "UpdateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserRequest);
            if (updatedUser == null)
            {
                return NotFound(ApiResponse<UserDto>.Fail("User not found."));
            }
            _linkService.AddLinks(updatedUser);
            return Ok(ApiResponse<UserDto>.Success(updatedUser));
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(ApiResponse<object>.Success(new { message = "User deleted successfully." }));
        }

    }
}