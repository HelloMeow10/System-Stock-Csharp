namespace AgileStockPro.App.Models;

public record Almacen(
    string Id,
    string Name,
    string Location,
    int Capacity,
    int ProductCount
);