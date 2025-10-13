using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
}
