using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services.Api;

/// <summary>
/// API-backed data service. Falls back to the local AppDataService for areas not yet implemented.
/// </summary>
public class ApiAppDataService : IAppDataService
{
    private readonly BackendApiClient _api;
    private readonly AppDataService _fallback;

    public ApiAppDataService(BackendApiClient api, AppDataService fallback)
    {
        _api = api;
        _fallback = fallback;
    }

    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken ct = default) => _fallback.GetSuppliersAsync(ct);
    public Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default) => _fallback.GetCustomersAsync(ct);
    public Task<IReadOnlyList<Almacen>> GetWarehousesAsync(CancellationToken ct = default) => _fallback.GetWarehousesAsync(ct);
    public Task<IReadOnlyList<StockItem>> GetStockAsync(CancellationToken ct = default) => _fallback.GetStockAsync(ct);
    public Task<IReadOnlyList<OrdenCompra>> GetPurchaseOrdersAsync(CancellationToken ct = default) => _fallback.GetPurchaseOrdersAsync(ct);
    public Task<IReadOnlyList<Pedido>> GetOrdersAsync(CancellationToken ct = default) => _fallback.GetOrdersAsync(ct);
    public Task<IReadOnlyList<Factura>> GetInvoicesAsync(CancellationToken ct = default) => _fallback.GetInvoicesAsync(ct);
    public Task<IReadOnlyList<Alerta>> GetAlertsAsync(CancellationToken ct = default) => _fallback.GetAlertsAsync(ct);

    public async Task<IReadOnlyList<Movimiento>> GetMovementsAsync(CancellationToken ct = default)
    {
        var to = DateOnly.FromDateTime(DateTime.Now);
        var from = to.AddDays(-30);
        var data = await _api.GetAsync<IEnumerable<StockMovementDto>>($"api/v1/stock/movements?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
        return data.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<ScrapItem>> GetScrapAsync(CancellationToken ct = default)
    {
        var to = DateOnly.FromDateTime(DateTime.Now);
        var from = to.AddDays(-30);
        var data = await _api.GetAsync<IEnumerable<ScrapReportItemDto>>($"api/v1/stock/scrap?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
        return data.Select(Map).ToList();
    }

    private static Movimiento Map(StockMovementDto dto) => new()
    {
        Id = dto.Id,
        Date = dto.Fecha,
        Type = dto.TipoMovimiento,
        Product = dto.Producto,
        Quantity = dto.Cantidad,
        From = dto.Usuario, // Origen no provisto; mostramos el usuario como referencia
        To = null
    };

    private static ScrapItem Map(ScrapReportItemDto dto) => new()
    {
        Id = dto.Id,
        Date = dto.Fecha,
        Product = dto.Producto,
        Quantity = dto.Cantidad,
        Reason = dto.Motivo
    };

    // Backend DTOs (mirror backend contracts for this feature)
    public class StockMovementDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ScrapReportItemDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
