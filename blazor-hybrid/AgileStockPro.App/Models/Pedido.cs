namespace AgileStockPro.App.Models;

public record Pedido(
    string Id,
    string Fecha,
    string Cliente,
    int Items,
    decimal Total,
    string Estado
);