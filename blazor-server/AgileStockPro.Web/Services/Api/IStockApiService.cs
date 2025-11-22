using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services.Api;

public interface IStockApiService
{
    Task<IReadOnlyList<ProductPickVm>> GetProductsAsync(string? search = null, int pageSize = 200, CancellationToken ct = default);
    Task<IReadOnlyList<ScrapReasonVm>> GetScrapReasonsAsync(CancellationToken ct = default);
    Task CreateScrapAsync(int productId, int cantidad, int usuarioId, int motivoScrapId, CancellationToken ct = default);
    Task IngresoAsync(IngresoMercaderiaVm request, CancellationToken ct = default);
    Task<IReadOnlyList<Movimiento>> GetMovementsAsync(DateOnly from, DateOnly to);
    Task<IReadOnlyList<ScrapItem>> GetScrapAsync(DateOnly from, DateOnly to);
}

public record ProductPickVm(int Id, string Codigo, string Nombre);
public record ScrapReasonVm(int Id, string Descripcion);
public record IngresoMercaderiaVm(
    int ProductoId,
    int UsuarioId,
    int Cantidad,
    string? Lote,
    int? StockMinimo,
    int? StockIdeal,
    int? StockMaximo,
    string? TipoStock,
    int? PuntoReposicion,
    DateOnly? FechaVencimiento,
    string? EstadoHabilitaciones,
    int? MovimientoStockId
);
