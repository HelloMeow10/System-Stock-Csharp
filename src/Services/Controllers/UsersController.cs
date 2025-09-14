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

            AddLinksToUser(updatedUser);

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

            var links = CreateLinksForCollection(pagedUsers, "GetUsers", paginationParams);
            var linkedUsers = pagedUsers.Items.Select(user =>
            {
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

            AddLinksToUser(user);

            return Ok(user);
        }

        private void AddLinksToUser(UserDto user)
        {
            user.Links.Add(CreateLink("GetUserById", new { id = user.IdUsuario }, "self", "GET"));
            user.Links.Add(CreateLink("DeleteUser", new { id = user.IdUsuario }, "delete_user", "DELETE"));
            user.Links.Add(CreateLink("UpdateUser", new { id = user.IdUsuario }, "update_user", "PUT"));
        }

        private List<LinkDto> CreateLinksForCollection<T>(PagedList<T> pagedList, string routeName, PaginationParams paginationParams)
        {
            var links = new List<LinkDto>
            {
                CreateLink(routeName, new { pageNumber = paginationParams.PageNumber, pageSize = paginationParams.PageSize }, "self", "GET")
            };

            if (pagedList.HasNext)
            {
                links.Add(CreateLink(routeName, new { pageNumber = pagedList.CurrentPage + 1, pageSize = pagedList.PageSize }, "nextPage", "GET"));
            }

            if (pagedList.HasPrevious)
            {
                links.Add(CreateLink(routeName, new { pageNumber = pagedList.CurrentPage - 1, pageSize = pagedList.PageSize }, "previousPage", "GET"));
            }

            return links;
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
        /// Fully updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">The user data for the update.</param>
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
        public async Task<IActionResult> Put(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.IdUsuario)
            {
                return BadRequest("User ID in the URL must match the ID in the request body.");
            }

            var updatedUser = await _userService.UpdateUserAsync(userDto);
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