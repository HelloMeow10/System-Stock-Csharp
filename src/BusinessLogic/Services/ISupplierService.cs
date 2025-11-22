using Contracts;

namespace BusinessLogic.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(int id);
        Task<IEnumerable<SupplierDto>> SearchAsync(string term);
        Task CreateAsync(CreateSupplierRequest request);
        Task UpdateAsync(UpdateSupplierRequest request);
        Task DeleteAsync(int id);
        Task AddContactAsync(int supplierId, string phone, string sector, string schedule, string email);
        Task AddAddressAsync(int supplierId, string address, string city, string province, string type);
    }
}
