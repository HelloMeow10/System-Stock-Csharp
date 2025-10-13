using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface IOrdenCompraService
{
    Task<IEnumerable<OrdenCompra>> GetOrdenesCompraAsync();
}