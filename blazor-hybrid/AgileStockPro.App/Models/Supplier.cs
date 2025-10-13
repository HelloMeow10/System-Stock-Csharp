namespace AgileStockPro.App.Models;

public record Supplier(
    string Id,
    string Name,
    string Cuit,
    string Phone,
    string Email,
    string Address,
    IReadOnlyList<string> Categories,
    string Discount,
    string DeliveryTime
);