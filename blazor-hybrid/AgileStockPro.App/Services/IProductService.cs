using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
}