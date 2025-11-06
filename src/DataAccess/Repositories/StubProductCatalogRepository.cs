using Contracts;

namespace DataAccess.Repositories;

/// <summary>
/// Temporary in-memory repository for Product Catalog while DB schema is finalized.
/// Replace with SqlProductCatalogRepository using DatabaseConnectionFactory.
/// </summary>
public class StubProductCatalogRepository : IProductCatalogRepository
{
    private readonly List<CategoryDto> _categories = new()
    {
        new CategoryDto(1, "Ferretería"),
        new CategoryDto(2, "Pinturería"),
        new CategoryDto(3, "Electricidad"),
        new CategoryDto(4, "Construcción"),
        new CategoryDto(5, "Iluminación"),
    };

    private readonly List<BrandDto> _brands = new()
    {
        new BrandDto(1, "Fischer"),
        new BrandDto(2, "Alba"),
        new BrandDto(3, "Belden"),
        new BrandDto(4, "Holcim"),
        new BrandDto(5, "Philips"),
    };

    private readonly List<ProductDto> _products;

    public StubProductCatalogRepository()
    {
        // Generate a richer catalog: 200 products distributed across categories/brands
        _products = new List<ProductDto>();
        var rnd = new Random(1234);
        var catNames = _categories.Select(c => c.Nombre).ToArray();
        var brandNames = _brands.Select(b => b.Nombre).ToArray();

        // Keep a few hand-crafted examples first
        _products.Add(new ProductDto(1, "P001", "Tornillo M8 x 40mm", "Ferretería", "Fischer", 250.00m, 150, 100, 500));
        _products.Add(new ProductDto(2, "P002", "Pintura Latex Blanca 20L", "Pinturería", "Alba", 38500.00m, 25, 50, 200));
        _products.Add(new ProductDto(3, "P003", "Cable UTP Cat6 305m", "Electricidad", "Belden", 152000.00m, 5, 10, 50));
        _products.Add(new ProductDto(4, "P004", "Cemento Portland 50kg", "Construcción", "Holcim", 7100.00m, 450, 200, 1000));
        _products.Add(new ProductDto(5, "P005", "Luminaria LED 18W", "Iluminación", "Philips", 4800.00m, 85, 100, 300));

        for (int i = 6; i <= 200; i++)
        {
            var cat = catNames[rnd.Next(catNames.Length)];
            var brand = brandNames[rnd.Next(brandNames.Length)];
            var codigo = $"P{i:000}";
            var nombre = $"Producto {i} {cat}";
            var precio = Math.Round((decimal)(rnd.NextDouble() * 200000 + 1000), 2);
            var stockMin = rnd.Next(5, 200);
            var stockMax = stockMin + rnd.Next(50, 800);
            var stockAct = rnd.Next(0, stockMax + 1);
            _products.Add(new ProductDto(i, codigo, nombre, cat, brand, precio, stockAct, stockMin, stockMax));
        }
    }

    public Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<BrandDto>>(_brands);

    public Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<CategoryDto>>(_categories);

    public Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult<ProductDto?>(_products.FirstOrDefault(p => p.Id == id));

    public Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        IEnumerable<ProductDto> query = _products;
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLowerInvariant();
            query = query.Where(p =>
                p.Codigo.ToLowerInvariant().Contains(search) ||
                p.Nombre.ToLowerInvariant().Contains(search) ||
                p.Categoria.ToLowerInvariant().Contains(search) ||
                p.Marca.ToLowerInvariant().Contains(search));
        }

        var total = query.Count();
        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult((items.AsEnumerable(), total));
    }
}
