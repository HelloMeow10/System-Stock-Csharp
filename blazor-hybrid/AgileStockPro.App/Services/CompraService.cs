using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class CompraService : ICompraService
{
    public Task<CompraStats> GetCompraStatsAsync()
    {
        var stats = new CompraStats(
            ProductosActivos: 1248,
            Proveedores: 87,
            OrdenesDelMes: 145,
            OrdenesDelMesTrend: 12
        );
        return Task.FromResult(stats);
    }

    public Task<List<OrdenCompra>> GetRecentOrdenesCompraAsync()
    {
        var ordenes = new List<OrdenCompra>
        {
            new("OC-2024-0156", "Ferretería Central SA", 45200, "Entregada", "15/03/2024", 8),
            new("OC-2024-0155", "Distribuidora del Norte", 28900, "Pendiente", "14/03/2024", 6),
            new("OC-2024-0154", "Pinturas y Revestimientos", 62400, "En tránsito", "13/03/2024", 12),
            new("OC-2024-0153", "Materiales Industriales", 18700, "Entregada", "12/03/2024", 5)
        };
        return Task.FromResult(ordenes);
    }
}