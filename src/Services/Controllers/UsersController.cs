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
using BusinessLogic.Mappers;
using Asp.Versioning;

using System.Linq;

namespace Services.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
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
        /// <response code="200">The user was successfully updated.</response>
        /// <response code="400">If the patch document is invalid or the model state is invalid after patching.</response>
        /// <response code="404">If the user to update is not found.</response>
        [HttpPatch("{id}", Name = "PatchUserV1")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var userToPatch = UserMapper.MapToUpdateUserRequest(user);

            patchDoc.ApplyTo(userToPatch, ModelState);
            TryValidateModel(userToPatch);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                throw new BusinessLogic.Exceptions.ValidationException(errors);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userToPatch);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Partially updates an existing user. (v2)
        /// </summary>
        [HttpPatch("{id}", Name = "PatchUserV2")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDtoV2>> PatchV2(int id, [FromBody] JsonPatchDocument<UpdateUserRequestV2> patchDoc)
        {
            var userV2 = await _userService.GetUserByIdAsyncV2(id);
            var userToPatch = new UpdateUserRequestV2
            {
                FullName = userV2.FullName,
                Correo = userV2.Correo,
                IdRol = userV2.IdRol,
                CambioContrasenaObligatorio = userV2.CambioContrasenaObligatorio,
                FechaExpiracion = userV2.FechaExpiracion,
                Habilitado = userV2.Habilitado
            };

            patchDoc.ApplyTo(userToPatch, ModelState);
            TryValidateModel(userToPatch);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                throw new BusinessLogic.Exceptions.ValidationException(errors);
            }

            var updatedUser = await _userService.UpdateUserAsyncV2(id, userToPatch);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Retrieves a paginated list of users. (v1)
        /// </summary>
        /// <param name="queryParameters">The pagination, filtering, and sorting parameters.</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        [HttpGet(Name = "GetUsersV1")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<UserDto>>> Get([FromQuery] UserQueryParameters queryParameters)
        {
            var users = await _userService.GetUsersAsync(queryParameters);
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a paginated list of users. (v2)
        /// </summary>
        /// <param name="queryParameters">The pagination, filtering, and sorting parameters.</param>
        /// <returns>A paginated list of users with HATEOAS links.</returns>
        [HttpGet(Name = "GetUsersV2")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<UserDtoV2>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<UserDtoV2>>> GetV2([FromQuery] UserQueryParameters queryParameters)
        {
            var users = await _userService.GetUsersAsyncV2(queryParameters);
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The requested user with HATEOAS links.</returns>
        [HttpGet("{id}", Name = "GetUserByIdV1")]
        [MapToApiVersion("1.0")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a specific user by their ID. (v2)
        /// </summary>
        [HttpGet("{id}", Name = "GetUserByIdV2")]
        [MapToApiVersion("2.0")]
        [Authorize]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDtoV2>> GetByIdV2(int id)
        {
            var user = await _userService.GetUserByIdAsyncV2(id);
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user data for creation.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost(Name = "CreateUserV1")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            return CreatedAtRoute("GetUserByIdV1", new { id = newUser.IdUsuario, version = "1.0" }, newUser);
        }

        /// <summary>
        /// Creates a new user. (v2)
        /// </summary>
        [HttpPost(Name = "CreateUserV2")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDtoV2>> PostV2([FromBody] UserRequestV2 userRequest)
        {
            var newUserV2 = await _userService.CreateUserAsyncV2(userRequest);
            return CreatedAtRoute("GetUserByIdV2", new { id = newUserV2.IdUsuario, version = "2.0" }, newUserV2);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserRequest">The user data for the update.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">The user was successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpPut("{id}", Name = "UpdateUserV1")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserRequest);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Fully updates an existing user. (v2)
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUserV2")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDtoV2>> PutV2(int id, [FromBody] UpdateUserRequestV2 updateUserRequest)
        {
            var updatedUser = await _userService.UpdateUserAsyncV2(id, updateUserRequest);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">The user was successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user. (v2)
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">The user was successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}", Name = "DeleteUserV2")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteV2(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}