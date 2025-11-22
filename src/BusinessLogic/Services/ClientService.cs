using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ClientDto>> SearchAsync(string term)
        {
            var byName = await _repository.SearchByNameAsync(term);
            var byCuit = await _repository.SearchByCuitAsync(term);
            
            return byName.Union(byCuit, new ClientDtoComparer()).ToList();
        }

        public async Task CreateAsync(CreateClientRequest request)
        {
            await _repository.AddAsync(request);
        }

        public async Task UpdateAsync(UpdateClientRequest request)
        {
            await _repository.UpdateAsync(request);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddContactAsync(int clientId, string phone, string sector, string schedule, string email)
        {
            await _repository.AddContactAsync(clientId, phone, sector, schedule, email);
        }

        public async Task AddAddressAsync(int clientId, string address, string city, string province, string type)
        {
            await _repository.AddAddressAsync(clientId, address, city, province, type);
        }

        private class ClientDtoComparer : IEqualityComparer<ClientDto>
        {
            public bool Equals(ClientDto? x, ClientDto? y)
            {
                if (x == null || y == null) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(ClientDto obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
