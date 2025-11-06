using Contracts;

namespace DataAccess.Repositories;

public interface IProductCatalogRepository
{
    Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default);
}
