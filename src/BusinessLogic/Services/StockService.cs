using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _repo;
    public StockService(IStockRepository repo) => _repo = repo;

    public Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default)
        => _repo.CreateScrapAsync(request, ct);

    public Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
        => _repo.GetMovementsAsync(from, to, ct);

    public Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
        => _repo.GetScrapAsync(from, to, ct);

    public Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default)
        => _repo.GetScrapReasonsAsync(ct);

    public Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default)
        => _repo.IngresoMercaderiaAsync(request, ct);

    public Task<IEnumerable<StockItemDto>> GetStockAsync(CancellationToken ct = default)
        => _repo.GetStockAsync(ct);
}
