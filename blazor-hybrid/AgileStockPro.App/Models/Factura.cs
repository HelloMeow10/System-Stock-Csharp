namespace AgileStockPro.App.Models;

public enum EstadoFactura
{
    Pagada,
    Pendiente,
    Aplicada,
    Vencida
}

public record Factura(
    string Id,
    DateOnly Fecha,
    string Cliente,
    string Tipo,
    decimal Total,
    EstadoFactura Estado
);