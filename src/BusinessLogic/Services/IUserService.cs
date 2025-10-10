using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using SharedKernel;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserRequest request);
        Task<UserDtoV2> CreateUserAsyncV2(UserRequestV2 request);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest userDto);
        Task DeleteUserAsync(int userId);
        Task<PagedResponse<T>> GetUsersAsync<T>(UserQueryParameters queryParameters) where T : class;
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<T> GetUserByIdAsync<T>(int id) where T : class;
        Task<UserDtoV2> UpdateUserAsyncV2(int id, UpdateUserRequestV2 updateUserRequest);
    }
}
