using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string term)
        {
            // Search by name or CUIT
            var byName = await _repository.SearchByNameAsync(term);
            var byCuit = await _repository.SearchByCuitAsync(term);
            
            return byName.Union(byCuit, new SupplierDtoComparer()).ToList();
        }

        public async Task CreateAsync(CreateSupplierRequest request)
        {
            await _repository.AddAsync(request);
        }

        public async Task UpdateAsync(UpdateSupplierRequest request)
        {
            await _repository.UpdateAsync(request);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddContactAsync(int supplierId, string phone, string sector, string schedule, string email)
        {
            await _repository.AddPhoneAsync(supplierId, phone, sector, schedule, email);
        }

        public async Task AddAddressAsync(int supplierId, string address, string city, string province, string type)
        {
            await _repository.AddAddressAsync(supplierId, address, city, province, type);
        }

        private class SupplierDtoComparer : IEqualityComparer<SupplierDto>
        {
            public bool Equals(SupplierDto? x, SupplierDto? y)
            {
                if (x == null || y == null) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(SupplierDto obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
