using AgileStockPro.Web.Models;
using AgileStockPro.Web.Services.Api;

namespace AgileStockPro.Web.Services;

// API-backed dashboard service
public class DashboardService : IDashboardService
{
    private readonly BackendApiClient _api;

    public DashboardService(BackendApiClient api)
    {
        _api = api;
    }

    public async Task<IReadOnlyList<StatsCardModel>> GetStatsAsync(CancellationToken ct = default)
    {
        // GET summary stats
        var dto = await _api.GetAsync<DashboardSummaryDto>("api/v1/dashboard/summary");
        var list = new List<StatsCardModel>
        {
            new() { Title = "Ventas", Subtitle = "Hoy", Value = dto.SalesTodayDisplay, Delta = dto.SalesDeltaDisplay, DeltaAria = dto.SalesDeltaAria, Accent = "success", Icon = "chart-line" },
            new() { Title = "Compras", Subtitle = "Hoy", Value = dto.PurchasesTodayDisplay, Delta = dto.PurchasesDeltaDisplay, DeltaAria = dto.PurchasesDeltaAria, Accent = "accent", Icon = "cart" },
            new() { Title = "Stock", Subtitle = "Disponible", Value = dto.StockAvailableDisplay, Delta = dto.StockDeltaDisplay, DeltaAria = dto.StockDeltaAria, Accent = "warning", Icon = "box" },
            new() { Title = "Alertas", Subtitle = "Activas", Value = dto.AlertsActiveDisplay, Delta = dto.AlertsDeltaDisplay, DeltaAria = dto.AlertsDeltaAria, Accent = "danger", Icon = "alert" },
        };
        return list;
    }

    public async Task<IReadOnlyList<RecentActivityModel>> GetRecentActivitiesAsync(CancellationToken ct = default)
    {
        var items = await _api.GetAsync<List<RecentActivityDto>>("api/v1/dashboard/recent");
        return items.Select(i => new RecentActivityModel { When = i.When, Description = i.Description, Category = i.Category }).ToList();
    }

    public async Task<IReadOnlyList<TopProductModel>> GetTopProductsAsync(CancellationToken ct = default)
    {
        var items = await _api.GetAsync<List<TopProductDto>>("api/v1/dashboard/top-products");
        return items.Select(p => new TopProductModel { Name = p.Name, Category = p.Category, Sold = p.Sold, Stock = p.Stock }).ToList();
    }
}
