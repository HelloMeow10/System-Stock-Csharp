using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services;

public class ProductCatalogService : IProductCatalogService
{
    private readonly IProductCatalogRepository _repo;

    public ProductCatalogService(IProductCatalogRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default)
        => await _repo.GetBrandsAsync(ct);

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
        => await _repo.GetCategoriesAsync(ct);

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
        => await _repo.GetProductByIdAsync(id, ct);

    public async Task<PagedResponse<ProductDto>> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var (items, total) = await _repo.GetProductsAsync(search, pageNumber, pageSize, ct);
        return new PagedResponse<ProductDto>(items, pageNumber, pageSize, total);
    }
}
