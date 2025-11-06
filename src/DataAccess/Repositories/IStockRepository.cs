using Contracts;

namespace DataAccess.Repositories;

public interface IStockRepository
{
    Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default);
    Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default);
    Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default);
}
