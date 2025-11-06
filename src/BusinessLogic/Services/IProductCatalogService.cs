using Contracts;

namespace BusinessLogic.Services;

public interface IProductCatalogService
{
    Task<PagedResponse<ProductDto>> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default);
}
