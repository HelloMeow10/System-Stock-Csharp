namespace AgileStockPro.App.Models;

public record Venta(
    string Id,
    string Cliente,
    string Total,
    string Fecha,
    string Estado
);