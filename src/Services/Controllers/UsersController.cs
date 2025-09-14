using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Services.Hateoas;
using Microsoft.AspNetCore.JsonPatch;

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
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPatch("{id}", Name = "PatchUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("A patch document is required.");
            }

            var userToUpdate = await _userService.GetUserByIdAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(userToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(userToUpdate);

            _linkService.AddLinksForUser(Url, updatedUser);

            return Ok(updatedUser);
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters (pageNumber, pageSize).</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        /// <response code="200">Returns the paginated list of users.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var pagedUsers = await _userService.GetUsersAsync(paginationParams);

            var paginationMetadata = new
            {
                pagedUsers.TotalCount,
                pagedUsers.PageSize,
                pagedUsers.CurrentPage,
                pagedUsers.TotalPages,
                pagedUsers.HasNext,
                pagedUsers.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(Url, pagedUsers, "GetUsers", paginationParams);
            var linkedUsers = pagedUsers.Items.Select(user => {
                _linkService.AddLinksForUser(Url, user);
                return user;
            });

            var result = new
            {
                value = linkedUsers,
                links
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The requested user with HATEOAS links.</returns>
        /// <response code="200">Returns the requested user.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _linkService.AddLinksForUser(Url, user);

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user data for creation.</param>
        /// <returns>The newly created user.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            return CreatedAtAction(nameof(GetById), new { id = newUser.IdUsuario }, newUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateRequest">The user data for the update.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the user was successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the user to update is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{id}", Name = "UpdateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserRequest updateRequest)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateRequest);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the user was successfully deleted.</response>
        /// <response code="404">If the user to delete is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}