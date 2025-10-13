using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface ICompraService
{
    Task<CompraStats> GetCompraStatsAsync();
    Task<List<OrdenCompra>> GetRecentOrdenesCompraAsync();
}