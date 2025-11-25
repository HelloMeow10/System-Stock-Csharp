namespace AgileStockPro.Web.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Cuit { get; set; }
    public string? Address { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public string? IvaCondition { get; set; }
    public int PaymentTermDays { get; set; }
    public string? Observations { get; set; }
    public List<SupplierContact> Contacts { get; set; } = new();
}

public class SupplierContact
{
    public string Name { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class Almacen
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public class StockItem
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public string Warehouse { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
}

public class Movimiento
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty; // Entrada/Salida/Transferencia
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}

public class OrdenCompra
{
    public int Id { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Pendiente"; // Pendiente/Recibida/Cancelada
    public decimal Total { get; set; }
}

public class Pedido
{
    public int Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Preparando"; // Preparando/Enviado/Entregado
    public decimal Total { get; set; }
}

public class Factura
{
    public int Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Emitida"; // Emitida/Pagada/Vencida
}

public class Alerta
{
    public DateTime Date { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = "warning"; // info/warning/danger
}

public class ScrapItem
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
