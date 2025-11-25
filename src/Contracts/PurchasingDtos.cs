namespace Contracts;

public class PurchaseQuoteDto
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public string? Proveedor { get; set; }
    public string? ProveedorCUIT { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public int Items { get; set; }
}

public class PurchaseQuoteItemDto
{
    public int Id { get; set; }
    public int PresupuestoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}

public class PurchaseOrderDto
{
    public int Id { get; set; }
    public int PresupuestoId { get; set; }
    public int ProveedorId { get; set; }
    public string? Proveedor { get; set; }
    public string? ProveedorCUIT { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public bool Entregado { get; set; }
    public int Items { get; set; }
}

public class PurchaseOrderItemDto
{
    public int Id { get; set; }
    public int OrdenCompraId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int? RecibidoCantidad { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}
