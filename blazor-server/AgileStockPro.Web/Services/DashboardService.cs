using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public class DashboardService : IDashboardService
{
    public async Task<IReadOnlyList<StatsCardModel>> GetStatsAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<StatsCardModel>
        {
            new() { Title = "Ventas", Subtitle = "Hoy", Value = "$12,430", Delta = "+8.2%", DeltaAria = "Subió 8.2 por ciento comparado a ayer", Accent = "success", Icon = "chart-line" },
            new() { Title = "Compras", Subtitle = "Hoy", Value = "$7,980", Delta = "+3.4%", DeltaAria = "Subió 3.4 por ciento", Accent = "accent", Icon = "cart" },
            new() { Title = "Stock", Subtitle = "Disponible", Value = "1,284", Delta = "-1.1%", DeltaAria = "Bajó 1.1 por ciento", Accent = "warning", Icon = "box" },
            new() { Title = "Alertas", Subtitle = "Activas", Value = "5", Delta = "+2", DeltaAria = "Dos alertas nuevas", Accent = "danger", Icon = "alert" },
        };
    }

    public async Task<IReadOnlyList<RecentActivityModel>> GetRecentActivitiesAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        var now = DateTime.Now;
        return new List<RecentActivityModel>
        {
            new() { When = now.AddMinutes(-12), Description = "Venta #10231 a Cliente ACME ($340)", Category = "sale" },
            new() { When = now.AddMinutes(-25), Description = "Compra OC-5543 recibida (24u)", Category = "purchase" },
            new() { When = now.AddMinutes(-41), Description = "Stock ajustado de Producto X (-3)", Category = "stock" },
            new() { When = now.AddHours(-2), Description = "Alerta: Bajo stock en Cable USB", Category = "alert" },
            new() { When = now.AddHours(-3), Description = "Pedido #50012 preparado para despacho", Category = "order" },
        };
    }

    public async Task<IReadOnlyList<TopProductModel>> GetTopProductsAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<TopProductModel>
        {
            new() { Name = "Mouse Inalámbrico", Category = "Periféricos", Sold = 128, Stock = 42 },
            new() { Name = "Teclado Mecánico", Category = "Periféricos", Sold = 95, Stock = 18 },
            new() { Name = "Cable USB-C", Category = "Accesorios", Sold = 210, Stock = 12 },
            new() { Name = "Monitor 24''", Category = "Monitores", Sold = 44, Stock = 7 },
            new() { Name = "Silla Gamer", Category = "Mobiliario", Sold = 22, Stock = 3 },
        };
    }
}
