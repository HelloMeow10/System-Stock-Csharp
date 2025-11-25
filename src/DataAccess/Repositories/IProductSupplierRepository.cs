using Contracts;

namespace DataAccess.Repositories
{
    public interface IProductSupplierRepository
    {
        Task RelateAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null);
        Task<IEnumerable<ProductSupplierRelationDto>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<ProductSupplierRelationDto>> GetSuppliersByProductAsync(int productId);
        Task DeleteRelationAsync(int productId, int supplierId);
        Task UpdateRelationAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null);
    }
}
