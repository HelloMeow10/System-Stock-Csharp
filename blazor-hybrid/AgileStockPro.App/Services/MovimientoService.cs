using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class MovimientoService
{
    private readonly List<Movimiento> _movimientos = new()
    {
        new Movimiento("MOV-2024-1234", TipoMovimiento.Entrada, "RMT-8842", "Tornillo M8 x 40mm", 150, new DateTime(2024, 3, 15, 10, 30, 0), "Admin"),
        new Movimiento("MOV-2024-1233", TipoMovimiento.Salida, "NP-2024-0456", "Pintura Latex 20L", -25, new DateTime(2024, 3, 15, 9, 15, 0), "Ventas1"),
        new Movimiento("MOV-2024-1232", TipoMovimiento.Entrada, "RMT-8841", "Cable UTP Cat6", 50, new DateTime(2024, 3, 14, 16, 45, 0), "Admin"),
        new Movimiento("MOV-2024-1231", TipoMovimiento.Salida, "NP-2024-0455", "Cemento Portland 50kg", -200, new DateTime(2024, 3, 14, 14, 20, 0), "Ventas2"),
        new Movimiento("MOV-2024-1230", TipoMovimiento.Entrada, "RMT-8840", "Luminaria LED 18W", 100, new DateTime(2024, 3, 13, 11, 0, 0), "Admin"),
    };

    public Task<List<Movimiento>> ObtenerMovimientosAsync()
    {
        return Task.FromResult(_movimientos);
    }
}