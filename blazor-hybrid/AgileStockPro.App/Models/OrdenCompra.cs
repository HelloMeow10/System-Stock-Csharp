namespace AgileStockPro.App.Models;

public record OrdenCompra(
    string Id,
    string Proveedor,
    decimal Total,
    string Estado,
    string Fecha,
    int Items
);