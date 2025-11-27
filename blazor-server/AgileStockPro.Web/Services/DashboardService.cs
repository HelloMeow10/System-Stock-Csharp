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
        try
        {
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
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            // Backend endpoint not available yet: return safe defaults
            return new List<StatsCardModel>
            {
                new() { Title = "Ventas", Subtitle = "Hoy", Value = "0", Delta = string.Empty, Accent = "success", Icon = "chart-line" },
                new() { Title = "Compras", Subtitle = "Hoy", Value = "0", Delta = string.Empty, Accent = "accent", Icon = "cart" },
                new() { Title = "Stock", Subtitle = "Disponible", Value = "0", Delta = string.Empty, Accent = "warning", Icon = "box" },
                new() { Title = "Alertas", Subtitle = "Activas", Value = "0", Delta = string.Empty, Accent = "danger", Icon = "alert" },
            };
        }
    }

    public async Task<IReadOnlyList<RecentActivityModel>> GetRecentActivitiesAsync(CancellationToken ct = default)
    {
        try
        {
            var items = await _api.GetAsync<List<RecentActivityDto>>("api/v1/dashboard/recent");
            return items.Select(i => new RecentActivityModel { When = i.When, Description = i.Description, Category = i.Category }).ToList();
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return Array.Empty<RecentActivityModel>();
        }
    }

    public async Task<IReadOnlyList<TopProductModel>> GetTopProductsAsync(CancellationToken ct = default)
    {
        try
        {
            var items = await _api.GetAsync<List<TopProductDto>>("api/v1/dashboard/top-products");
            return items.Select(p => new TopProductModel { Name = p.Name, Category = p.Category, Sold = p.Sold, Stock = p.Stock }).ToList();
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return Array.Empty<TopProductModel>();
        }
    }
}
