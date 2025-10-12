using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class FacturacionService
{
    private readonly List<Factura> _facturas = new()
    {
        new Factura("FC-A-0001234", new DateOnly(2024, 3, 15), "Construcciones del Sur", "Factura A", 15200m, EstadoFactura.Pagada),
        new Factura("FC-B-0002345", new DateOnly(2024, 3, 15), "Obras Civiles SA", "Factura B", 28400m, EstadoFactura.Pendiente),
        new Factura("NC-A-0000123", new DateOnly(2024, 3, 14), "Instalaciones Industriales", "Nota Crédito", -2800m, EstadoFactura.Aplicada),
        new Factura("FC-A-0001233", new DateOnly(2024, 3, 14), "Arquitectura Moderna", "Factura A", 42600m, EstadoFactura.Pagada),
        new Factura("FC-B-0002344", new DateOnly(2024, 3, 13), "Construcciones del Sur", "Factura B", 11300m, EstadoFactura.Vencida),
    };

    public Task<List<Factura>> ObtenerFacturasAsync()
    {
        return Task.FromResult(_facturas);
    }
}