using Contracts;

namespace BusinessLogic.Services;

public interface IStockService
{
    Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default);
    Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default);
    Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default);
}
