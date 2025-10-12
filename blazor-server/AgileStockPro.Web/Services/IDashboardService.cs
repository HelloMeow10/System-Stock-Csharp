using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public interface IDashboardService
{
    Task<IReadOnlyList<StatsCardModel>> GetStatsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RecentActivityModel>> GetRecentActivitiesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TopProductModel>> GetTopProductsAsync(CancellationToken ct = default);
}
