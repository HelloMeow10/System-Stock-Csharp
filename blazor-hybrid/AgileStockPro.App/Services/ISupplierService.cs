using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface ISupplierService
{
    Task<List<Supplier>> GetSuppliersAsync();
}