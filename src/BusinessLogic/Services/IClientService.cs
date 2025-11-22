using Contracts;

namespace BusinessLogic.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
        Task<IEnumerable<ClientDto>> SearchAsync(string term);
        Task CreateAsync(CreateClientRequest request);
        Task UpdateAsync(UpdateClientRequest request);
        Task DeleteAsync(int id);
        Task AddContactAsync(int clientId, string phone, string sector, string schedule, string email);
        Task AddAddressAsync(int clientId, string address, string city, string province, string type);
    }
}
