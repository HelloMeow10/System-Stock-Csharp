using Asp.Versioning;
using AutoMapper;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Threading.Tasks;

namespace Services.Controllers.V1
{
    [ApiVersion("1.0")]
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
        /// Retrieves a paginated list of users.
        /// </summary>
        [HttpGet(Name = "GetUsersV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
        public Task<PagedResponse<UserDto>> Get([FromQuery] UserQueryParameters queryParameters)
        {
            return _userService.GetUsersAsync<UserDto>(queryParameters);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        [HttpGet("{id}", Name = "GetUserByIdV1")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDto> GetById(int id)
        {
            return _userService.GetUserByIdAsync<UserDto>(id);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost(Name = "CreateUserV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync<UserRequest, UserDto>(userRequest);
            return CreatedAtAction(nameof(GetById), new { id = newUser.IdUsuario, version = "1.0" }, newUser);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUserV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDto> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            return _userService.UpdateUserAsync<UpdateUserRequest, UserDto>(id, updateUserRequest);
        }

        /// <summary>
        /// Partially updates an existing user.
        /// </summary>
        [HttpPatch("{id}", Name = "PatchUserV1")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDto> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            return _userService.PatchUserAsync<UpdateUserRequest, UserDto>(id, patchDoc);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteUserV1")]
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