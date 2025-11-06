using System.Text.Json;
using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services.Api;

public class StockApiService : IStockApiService
{
    private readonly BackendApiClient _api;
    private static readonly JsonSerializerOptions _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    public StockApiService(BackendApiClient api) => _api = api;

    public async Task<IReadOnlyList<ProductPickVm>> GetProductsAsync(string? search = null, int pageSize = 200, CancellationToken ct = default)
    {
        var url = $"api/v1/products?pageNumber=1&pageSize={pageSize}" + (string.IsNullOrWhiteSpace(search) ? string.Empty : $"&search={Uri.EscapeDataString(search)}");
        var resp = await _api.GetAsync<PagedResponse<ProductDto>>(url);
        var items = resp.Items ?? Enumerable.Empty<ProductDto>();
        return items.Select(p => new ProductPickVm(p.Id, p.Codigo, p.Nombre)).ToList();
    }

    public async Task<IReadOnlyList<ScrapReasonVm>> GetScrapReasonsAsync(CancellationToken ct = default)
    {
        var reasons = await _api.GetAsync<IEnumerable<ScrapReasonDto>>("api/v1/stock/reasons");
        return reasons.Select(r => new ScrapReasonVm(r.Id, BuildDescripcion(r))).ToList();
    }

    public Task CreateScrapAsync(int productId, int cantidad, int usuarioId, int motivoScrapId, CancellationToken ct = default)
        => _api.PostAsync("api/v1/stock/scrap", new { ProductoId = productId, Cantidad = cantidad, UsuarioId = usuarioId, MotivoScrapId = motivoScrapId });

    public Task IngresoAsync(IngresoMercaderiaVm request, CancellationToken ct = default)
        => _api.PostAsync("api/v1/stock/ingreso", request);

    private static string BuildDescripcion(ScrapReasonDto r)
    {
        var parts = new List<string>();
        if (r.Dano) parts.Add("Daño");
        if (r.Vencido) parts.Add("Vencido");
        if (r.Obsoleto) parts.Add("Obsoleto");
        if (r.MalaCalidad) parts.Add("Mala calidad");
        return parts.Count == 0 ? "Otro" : string.Join(", ", parts);
    }

    // Backend DTO mirrors
    public class PagedResponse<T> { public IEnumerable<T>? Items { get; set; } public int PageNumber { get; set; } public int PageSize { get; set; } public int TotalRecords { get; set; } }
    public class ProductDto { public int Id { get; set; } public string Codigo { get; set; } = string.Empty; public string Nombre { get; set; } = string.Empty; }
    public class ScrapReasonDto { public int Id { get; set; } public bool Dano { get; set; } public bool Vencido { get; set; } public bool Obsoleto { get; set; } public bool MalaCalidad { get; set; } }

    // Added: read movements and scrap directly for date filters
    public async Task<IReadOnlyList<Movimiento>> GetMovementsAsync(DateOnly from, DateOnly to)
    {
        var list = await _api.GetAsync<IEnumerable<StockMovementDto>>($"api/v1/stock/movements?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
        return list.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<ScrapItem>> GetScrapAsync(DateOnly from, DateOnly to)
    {
        var list = await _api.GetAsync<IEnumerable<ScrapReportItemDto>>($"api/v1/stock/scrap?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
        return list.Select(Map).ToList();
    }

    private static Movimiento Map(StockMovementDto dto) => new()
    {
        Id = dto.Id,
        Date = dto.Fecha,
        Type = dto.TipoMovimiento,
        Product = dto.Producto,
        Quantity = dto.Cantidad,
        From = dto.Usuario,
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

    public class StockMovementDto { public int Id { get; set; } public DateTime Fecha { get; set; } public string Usuario { get; set; } = string.Empty; public string Producto { get; set; } = string.Empty; public string TipoMovimiento { get; set; } = string.Empty; public int Cantidad { get; set; } }
    public class ScrapReportItemDto { public int Id { get; set; } public DateTime Fecha { get; set; } public string Usuario { get; set; } = string.Empty; public string Producto { get; set; } = string.Empty; public string Motivo { get; set; } = string.Empty; public int Cantidad { get; set; } }
}
