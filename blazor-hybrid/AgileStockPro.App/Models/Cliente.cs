namespace AgileStockPro.App.Models;

public record Cliente(
    string Id,
    string Nombre,
    string Cuit,
    string Telefono,
    string Email,
    string Direccion,
    decimal LimiteCredito,
    decimal CreditoDisponible,
    int Ventas
);