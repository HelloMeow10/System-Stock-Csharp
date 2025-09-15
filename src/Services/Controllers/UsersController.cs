using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Services.Hateoas;
using BusinessLogic.Exceptions;

namespace Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            return await _userService.UpdateUserAsync(id, patchDoc);
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="queryParameters">The pagination, filtering, and sorting parameters.</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<IEnumerable<UserDto>>>> Get([FromQuery] UserQueryParameters queryParameters)
        {
            var pagedUsers = await _userService.GetUsersAsync(queryParameters);
            return new PagedResponse<IEnumerable<UserDto>>(pagedUsers.Items, pagedUsers.CurrentPage, pagedUsers.PageSize, pagedUsers.TotalCount);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The requested user with HATEOAS links.</returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user data for creation.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost(Name = "CreateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario }, newUser);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserRequest">The user data for the update.</param>
        /// <returns>The updated user.</returns>
        [HttpPut("{id}", Name = "UpdateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            return await _userService.UpdateUserAsync(id, updateUserRequest);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">The user was successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}