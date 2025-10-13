using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public interface IAppDataService
{
    Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Almacen>> GetWarehousesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StockItem>> GetStockAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Movimiento>> GetMovementsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<OrdenCompra>> GetPurchaseOrdersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Pedido>> GetOrdersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Factura>> GetInvoicesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Alerta>> GetAlertsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ScrapItem>> GetScrapAsync(CancellationToken ct = default);
}
