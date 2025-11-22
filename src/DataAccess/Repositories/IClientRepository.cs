using Contracts;

namespace DataAccess.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
        Task<IEnumerable<ClientDto>> SearchByNameAsync(string name);
        Task<IEnumerable<ClientDto>> SearchByCuitAsync(string cuit);
        Task AddAsync(CreateClientRequest client);
        Task UpdateAsync(UpdateClientRequest client);
        Task DeleteAsync(int id);
        Task AddContactAsync(int clientId, string phone, string sector, string schedule, string email);
        Task AddAddressAsync(int clientId, string address, string city, string province, string type);
    }
}
