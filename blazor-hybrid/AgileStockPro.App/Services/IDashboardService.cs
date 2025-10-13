using AgileStockPro.App.Models.Dashboard;

namespace AgileStockPro.App.Services;

public interface IDashboardService
{
    Task<DashboardStats> GetDashboardStatsAsync();
    Task<IEnumerable<AlertModel>> GetAlertsAsync();
    Task<IEnumerable<RecentActivityModel>> GetRecentActivityAsync();
    Task<IEnumerable<TopProductModel>> GetTopProductsAsync();
    Task<IEnumerable<TopSupplierModel>> GetTopSuppliersAsync();
    Task<IEnumerable<TopClientModel>> GetTopClientsAsync();
}