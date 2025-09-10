using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserRequest request);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int userId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}
