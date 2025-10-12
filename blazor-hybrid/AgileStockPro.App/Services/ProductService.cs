using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = "P001", Name = "Tornillo M8 x 40mm", Category = "Ferretería", Brand = "Fischer", Stock = 150, MinStock = 100, MaxStock = 500, Status = "ok" },
        new Product { Id = "P002", Name = "Pintura Latex Blanca 20L", Category = "Pinturería", Brand = "Alba", Stock = 25, MinStock = 50, MaxStock = 200, Status = "low" },
        new Product { Id = "P003", Name = "Cable UTP Cat6 305m", Category = "Electricidad", Brand = "Belden", Stock = 5, MinStock = 10, MaxStock = 50, Status = "critical" },
        new Product { Id = "P004", Name = "Cemento Portland 50kg", Category = "Construcción", Brand = "Holcim", Stock = 450, MinStock = 200, MaxStock = 1000, Status = "ok" },
        new Product { Id = "P005", Name = "Luminaria LED 18W", Category = "Iluminación", Brand = "Philips", Stock = 85, MinStock = 100, MaxStock = 300, Status = "low" },
    };

    public Task<IEnumerable<Product>> GetProductsAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products);
    }
}