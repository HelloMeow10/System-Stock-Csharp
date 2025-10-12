namespace AgileStockPro.App.Models;

public enum TipoMovimiento
{
    Entrada,
    Salida
}

public record Movimiento(
    string Id,
    TipoMovimiento Tipo,
    string Documento,
    string Producto,
    int Cantidad,
    DateTime FechaHora,
    string Usuario
);