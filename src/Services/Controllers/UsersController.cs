using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using SharedKernel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Asp.Versioning;
using AutoMapper;

namespace Services.Controllers
{
    [ApiVersion("1.0")]
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
        /// Partially updates an existing user.
        /// </summary>
        [HttpPatch("{id}", Name = "PatchUser")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<UserDto> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            var user = await _userService.GetUserByIdAsync<UserDto>(id);
            var userToPatch = _mapper.Map<UpdateUserRequest>(user);

            ApplyPatchAndValidate(patchDoc, userToPatch);

            return await _userService.UpdateUserAsync<UpdateUserRequest, UserDto>(id, userToPatch);
        }

        /// <summary>
        /// Partially updates an existing user. (v2)
        /// </summary>
        [HttpPatch("{id}", Name = "PatchUser")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<UserDtoV2> PatchV2(int id, [FromBody] JsonPatchDocument<UpdateUserRequestV2> patchDoc)
        {
            var userV2 = await _userService.GetUserByIdAsync<UserDtoV2>(id);
            var userToPatch = _mapper.Map<UpdateUserRequestV2>(userV2);

            ApplyPatchAndValidate(patchDoc, userToPatch);

            return await _userService.UpdateUserAsync<UpdateUserRequestV2, UserDtoV2>(id, userToPatch);
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// </summary>
        [HttpGet(Name = "GetUsers")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResponse<UserDtoV2>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] UserQueryParameters queryParameters)
        {
            var requestedVersion = HttpContext.GetRequestedApiVersion();

            if (requestedVersion?.MajorVersion == 2)
            {
                var users = await _userService.GetUsersAsync<UserDtoV2>(queryParameters);
                return Ok(users);
            }

            var usersV1 = await _userService.GetUsersAsync<UserDto>(queryParameters);
            return Ok(usersV1);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        [HttpGet("{id}", Name = "GetUserById")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var requestedVersion = HttpContext.GetRequestedApiVersion();

            if (requestedVersion?.MajorVersion == 2)
            {
                var user = await _userService.GetUserByIdAsync<UserDtoV2>(id);
                return Ok(user);
            }

            var userV1 = await _userService.GetUserByIdAsync<UserDto>(id);
            return Ok(userV1);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost(Name = "CreateUser")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.CreateUserAsync<UserRequest, UserDto>(userRequest);
            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario, version = "1.0" }, newUser);
        }

        /// <summary>
        /// Creates a new user. (v2)
        /// </summary>
        [HttpPost(Name = "CreateUser")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDtoV2>> PostV2([FromBody] UserRequestV2 userRequest)
        {
            var newUserV2 = await _userService.CreateUserAsync<UserRequestV2, UserDtoV2>(userRequest);
            return CreatedAtRoute("GetUserById", new { id = newUserV2.IdUsuario, version = "2.0" }, newUserV2);
        }

        /// <summary>
        /// Fully updates an existing user.
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUser")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDto> Put(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            return _userService.UpdateUserAsync<UpdateUserRequest, UserDto>(id, updateUserRequest);
        }

        /// <summary>
        /// Fully updates an existing user. (v2)
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUser")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<UserDtoV2> PutV2(int id, [FromBody] UpdateUserRequestV2 updateUserRequest)
        {
            return _userService.UpdateUserAsync<UpdateUserRequestV2, UserDtoV2>(id, updateUserRequest);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
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