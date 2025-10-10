using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using SharedKernel;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<TResponse> CreateUserAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;

        Task<TResponse> UpdateUserAsync<TRequest, TResponse>(int id, TRequest request)
            where TRequest : class
            where TResponse : class;

        Task<TResponse> PatchUserAsync<TRequest, TResponse>(int id, JsonPatchDocument<TRequest> patchDoc)
            where TRequest : class
            where TResponse : class;

        Task DeleteUserAsync(int userId);
        Task<PagedResponse<T>> GetUsersAsync<T>(UserQueryParameters queryParameters) where T : class;
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<T> GetUserByIdAsync<T>(int id) where T : class;
    }
}
