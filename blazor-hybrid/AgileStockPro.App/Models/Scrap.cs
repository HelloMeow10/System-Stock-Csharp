namespace AgileStockPro.App.Models;

public record Scrap(
    string Id,
    DateOnly Fecha,
    string Producto,
    int Cantidad,
    string Motivo,
    decimal ValorUnitario,
    string Usuario
)
{
    public decimal ValorTotal => Cantidad * ValorUnitario;
}