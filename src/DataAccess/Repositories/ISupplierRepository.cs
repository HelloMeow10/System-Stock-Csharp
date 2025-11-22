using Contracts;

namespace DataAccess.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(int id);
        Task<IEnumerable<SupplierDto>> SearchByNameAsync(string name);
        Task<IEnumerable<SupplierDto>> SearchByCuitAsync(string cuit);
        Task AddAsync(CreateSupplierRequest supplier);
        Task UpdateAsync(UpdateSupplierRequest supplier);
        Task DeleteAsync(int id);
        Task AddPhoneAsync(int supplierId, string phone, string sector, string schedule, string email);
        Task AddAddressAsync(int supplierId, string address, string city, string province, string type);
    }
}
