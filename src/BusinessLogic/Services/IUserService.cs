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
        Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest userDto);
        Task DeleteUserAsync(int userId);
        Task<PagedList<UserDto>> GetUsersAsync(UserQueryParameters queryParameters);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto> GetUserByIdAsync(int id);
    }
}
