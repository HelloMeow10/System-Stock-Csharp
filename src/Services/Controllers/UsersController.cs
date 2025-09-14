using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
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
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("A patch document is required.");
            }

            var userToPatch = await _userService.GetUserForUpdateAsync(id);
            if (userToPatch == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userToPatch);
            AddLinksToUser(updatedUser);

            return Ok(updatedUser);
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters (pageNumber, pageSize).</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
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

            var links = _linkService.GetLinksForCollection(pagedUsers, "GetUsers", paginationParams);

            var linkedUsers = pagedUsers.Items.Select(user => {
                AddLinksToUser(user);
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
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            AddLinksToUser(user);

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user data for creation.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost(Name = "CreateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            AddLinksToUser(newUser);

            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario }, newUser);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserRequest">The user data for the update.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut("{id}", Name = "UpdateUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            await _userService.UpdateUserAsync(id, updateUserRequest);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        private void AddLinksToUser(UserDto user)
        {
            var links = new List<LinkSpec>
            {
                new LinkSpec("GetUserById", new { id = user.IdUsuario }, "self", "GET"),
                new LinkSpec("DeleteUser", new { id = user.IdUsuario }, "delete_user", "DELETE"),
                new LinkSpec("UpdateUser", new { id = user.IdUsuario }, "update_user", "PUT"),
                new LinkSpec("PatchUser", new { id = user.IdUsuario }, "patch_user", "PATCH")
            };
            _linkService.AddLinksToResource(user, links);
        }
    }
}