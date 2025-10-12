using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class AlmacenService : IAlmacenService
{
    public Task<List<Almacen>> GetAlmacenesAsync()
    {
        var almacenes = new List<Almacen>
        {
            new("A-01", "Depósito Central", "Zona Norte, Buenos Aires", 5000, 1248),
            new("A-02", "Sucursal Córdoba", "Centro, Córdoba", 2500, 620),
            new("A-03", "Punto de Venta Rosario", "Zona Sur, Rosario", 1000, 310)
        };
        return Task.FromResult(almacenes);
    }
}