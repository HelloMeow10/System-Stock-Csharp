using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using SharedKernel;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserRequest request);
        Task<UserDtoV2> CreateUserAsyncV2(UserRequestV2 request);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest userDto);
        Task DeleteUserAsync(int userId);
        Task<PagedResponse<UserDto>> GetUsersAsync(UserQueryParameters queryParameters);
        Task<PagedResponse<UserDtoV2>> GetUsersAsyncV2(UserQueryParameters queryParameters);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDtoV2> GetUserByIdAsyncV2(int id);
        Task<UserDto> PatchUserAsync(int id, JsonPatchDocument<UpdateUserRequest> patchDoc);
        Task<UserDtoV2> UpdateUserAsyncV2(int id, UpdateUserRequestV2 updateUserRequest);
        Task<UserDtoV2> PatchUserAsyncV2(int id, JsonPatchDocument<UpdateUserRequestV2> patchDoc);
    }
}
