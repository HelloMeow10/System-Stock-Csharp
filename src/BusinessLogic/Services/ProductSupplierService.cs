using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public interface IProductSupplierService
    {
        Task RelateAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null);
        Task<IEnumerable<ProductSupplierRelationDto>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<ProductSupplierRelationDto>> GetSuppliersByProductAsync(int productId);
        Task DeleteRelationAsync(int productId, int supplierId);
        Task UpdateRelationAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null);
    }

    public class ProductSupplierService : IProductSupplierService
    {
        private readonly IProductSupplierRepository _repository;

        public ProductSupplierService(IProductSupplierRepository repository)
        {
            _repository = repository;
        }

        public Task RelateAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null) => _repository.RelateAsync(productId, supplierId, precioCompra, tiempoEntrega, descuento);
        public Task<IEnumerable<ProductSupplierRelationDto>> GetProductsBySupplierAsync(int supplierId) => _repository.GetProductsBySupplierAsync(supplierId);
        public Task<IEnumerable<ProductSupplierRelationDto>> GetSuppliersByProductAsync(int productId) => _repository.GetSuppliersByProductAsync(productId);
        public Task DeleteRelationAsync(int productId, int supplierId) => _repository.DeleteRelationAsync(productId, supplierId);
        public Task UpdateRelationAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null) => _repository.UpdateRelationAsync(productId, supplierId, precioCompra, tiempoEntrega, descuento);
    }
}
