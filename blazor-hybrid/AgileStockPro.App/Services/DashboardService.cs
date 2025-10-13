using AgileStockPro.App.Models.Dashboard;

namespace AgileStockPro.App.Services;

public class DashboardService : IDashboardService
{
    public Task<DashboardStats> GetDashboardStatsAsync()
    {
        var stats = new DashboardStats(
            TotalProducts: 1248,
            TotalProductsTrend: 12,
            LowStockProducts: 45,
            MonthlySales: 128450,
            MonthlySalesTrend: 8,
            PendingOrders: 23
        );
        return Task.FromResult(stats);
    }

    public Task<IEnumerable<AlertModel>> GetAlertsAsync()
    {
        var alerts = new List<AlertModel>
        {
            new("Tornillo M8 x 40mm", "SKU-4568", 52, 50, "Bajo Stock"),
            new("Pintura Latex 20L", "SKU-1234", 12, 10, "Bajo Stock"),
            new("Cable UTP Cat6", "SKU-7890", 5, 10, "Sin Stock"),
            new("Destornillador Phillips", "SKU-2468", 150, 20, "En Stock")
        };
        return Task.FromResult<IEnumerable<AlertModel>>(alerts);
    }

    public Task<IEnumerable<RecentActivityModel>> GetRecentActivityAsync()
    {
        var activities = new List<RecentActivityModel>
        {
            new("Nueva orden de compra", "OC-2024-0156 - Proveedor: Ferretería Central", "Hace 5 minutos"),
            new("Ingreso de mercadería", "Remito R-8842 - 150 unidades", "Hace 1 hora"),
            new("Venta registrada", "Cliente: Construcciones del Sur - $15,200", "Hace 2 horas"),
            new("Alerta de stock", "Producto P-445 alcanzó stock mínimo", "Hace 3 horas")
        };
        return Task.FromResult<IEnumerable<RecentActivityModel>>(activities);
    }

    public Task<IEnumerable<TopProductModel>> GetTopProductsAsync()
    {
        var products = new List<TopProductModel>
        {
            new("Tornillo M8 x 40mm", 850, "$12,400"),
            new("Pintura Latex 20L", 340, "$45,200"),
            new("Cable UTP Cat6", 210, "$28,900")
        };
        return Task.FromResult<IEnumerable<TopProductModel>>(products);
    }

    public Task<IEnumerable<TopSupplierModel>> GetTopSuppliersAsync()
    {
        var suppliers = new List<TopSupplierModel>
        {
            new("Ferretería Central SA", 45, "$85,400"),
            new("Distribuidora del Norte", 32, "$62,100"),
            new("Pinturas y Revestimientos", 28, "$48,900")
        };
        return Task.FromResult<IEnumerable<TopSupplierModel>>(suppliers);
    }

    public Task<IEnumerable<TopClientModel>> GetTopClientsAsync()
    {
        var clients = new List<TopClientModel>
        {
            new("Construcciones del Sur", 38, "$95,200"),
            new("Obras Civiles SA", 29, "$72,800"),
            new("Instalaciones Industriales", 24, "$58,400")
        };
        return Task.FromResult<IEnumerable<TopClientModel>>(clients);
    }
}