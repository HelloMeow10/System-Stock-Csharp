namespace AgileStockPro.App.Models;

public class PurchaseOrder
{
    public string? Id { get; set; }
    public DateOnly Fecha { get; set; }
    public string? Proveedor { get; set; }
    public decimal Total { get; set; }
    public string? Estado { get; set; }
    public int Items { get; set; }
}