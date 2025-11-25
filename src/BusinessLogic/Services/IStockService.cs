using Contracts;

using Contracts;

namespace BusinessLogic.Services;

public interface IStockService
{
    Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default);
    Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default);
    Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default);
    Task<IEnumerable<StockItemDto>> GetStockAsync(CancellationToken ct = default);
    Task<IEnumerable<StockValuationDto>> GetStockValuationAsync(CancellationToken ct = default);
    Task<IEnumerable<StockAlertDto>> GetAlertsAsync(CancellationToken ct = default);
}
