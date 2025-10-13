using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class OrdenCompraService : IOrdenCompraService
{
    private readonly List<OrdenCompra> _ordenesCompra = new()
    {
        new("OC-2024-0156", "Ferretería Central SA", 45200, "Entregada", "15/03/2024", 12),
        new("OC-2024-0155", "Distribuidora del Norte", 28900, "Pendiente", "14/03/2024", 8),
        new("OC-2024-0154", "Pinturas y Revestimientos", 62400, "En tránsito", "13/03/2024", 15),
        new("OC-2024-0153", "Materiales Industriales", 18700, "Entregada", "12/03/2024", 6),
        new("OC-2024-0152", "Distribuidora del Norte", 33500, "Cancelada", "11/03/2024", 10),
    };

    public Task<IEnumerable<OrdenCompra>> GetOrdenesCompraAsync()
    {
        return Task.FromResult<IEnumerable<OrdenCompra>>(_ordenesCompra);
    }
}