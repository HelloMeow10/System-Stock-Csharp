using Asp.Versioning;
using AutoMapper;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Threading.Tasks;

namespace Services.Controllers.V2
{
    [ApiVersion("2.0")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of users. (v2)
        /// </summary>
        [HttpGet(Name = "GetUsersV2")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<UserDtoV2>), StatusCodes.Status200OK)]
        public Task<PagedResponse<UserDtoV2>> Get([FromQuery] UserQueryParameters queryParameters)
        {
            return _userService.GetUsersAsync<UserDtoV2>(queryParameters);
        }

        /// <summary>
        /// Retrieves a specific user by their ID. (v2)
        /// </summary>
        [HttpGet("{id}", Name = "GetUserByIdV2")]
        [Authorize]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDtoV2> GetById(int id)
        {
            return _userService.GetUserByIdAsync<UserDtoV2>(id);
        }

        /// <summary>
        /// Creates a new user. (v2)
        /// </summary>
        [HttpPost(Name = "CreateUserV2")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDtoV2>> Post([FromBody] UserRequestV2 userRequest)
        {
            var newUser = await _userService.CreateUserAsync<UserRequestV2, UserDtoV2>(userRequest);
            return CreatedAtAction(nameof(GetById), new { id = newUser.IdUsuario, version = "2.0" }, newUser);
        }

        /// <summary>
        /// Fully updates an existing user. (v2)
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUserV2")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDtoV2> Put(int id, [FromBody] UpdateUserRequestV2 updateUserRequest)
        {
            return _userService.UpdateUserAsync<UpdateUserRequestV2, UserDtoV2>(id, updateUserRequest);
        }

        /// <summary>
        /// Partially updates an existing user. (v2)
        /// </summary>
        [HttpPatch("{id}", Name = "PatchUserV2")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<UserDtoV2> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequestV2> patchDoc)
        {
            var user = await _userService.GetUserByIdAsync<UserDtoV2>(id);
            var userToPatch = _mapper.Map<UpdateUserRequestV2>(user);

            ApplyPatchAndValidate(patchDoc, userToPatch);

            return await _userService.UpdateUserAsync<UpdateUserRequestV2, UserDtoV2>(id, userToPatch);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteUserV2")]
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