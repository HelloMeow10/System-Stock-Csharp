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

        private async Task<TResponse> PatchUserAsync<TRequest, TResponse>(int id, JsonPatchDocument<TRequest> patchDoc)
            where TRequest : class
            where TResponse : class
        {
            var user = await _userService.GetUserByIdAsync<TResponse>(id);
            var userToPatch = _mapper.Map<TRequest>(user);

            ApplyPatchAndValidate(patchDoc, userToPatch);

            return await _userService.UpdateUserAsync<TRequest, TResponse>(id, userToPatch);
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
        public Task<UserDto> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequest> patchDoc)
        {
            return PatchUserAsync<UpdateUserRequest, UserDto>(id, patchDoc);
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
        public Task<UserDtoV2> Patch(int id, [FromBody] JsonPatchDocument<UpdateUserRequestV2> patchDoc)
        {
            return PatchUserAsync<UpdateUserRequestV2, UserDtoV2>(id, patchDoc);
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
        public async Task<object> Get([FromQuery] UserQueryParameters queryParameters)
        {
            var requestedVersion = HttpContext.GetRequestedApiVersion();

            if (requestedVersion?.MajorVersion == 2)
            {
                return await _userService.GetUsersAsync<UserDtoV2>(queryParameters);
            }

            return await _userService.GetUsersAsync<UserDto>(queryParameters);
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
        public async Task<object> GetById(int id)
        {
            var requestedVersion = HttpContext.GetRequestedApiVersion();

            if (requestedVersion?.MajorVersion == 2)
            {
                return await _userService.GetUserByIdAsync<UserDtoV2>(id);
            }

            return await _userService.GetUserByIdAsync<UserDto>(id);
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
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario, version = apiVersion }, newUser);
        }

        /// <summary>
        /// Creates a new user. (v2)
        /// </summary>
        [HttpPost(Name = "CreateUser")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDtoV2), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDtoV2>> Post([FromBody] UserRequestV2 userRequest)
        {
            var newUser = await _userService.CreateUserAsync<UserRequestV2, UserDtoV2>(userRequest);
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            return CreatedAtRoute("GetUserById", new { id = newUser.IdUsuario, version = apiVersion }, newUser);
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
        public Task<UserDtoV2> Put(int id, [FromBody] UpdateUserRequestV2 updateUserRequest)
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