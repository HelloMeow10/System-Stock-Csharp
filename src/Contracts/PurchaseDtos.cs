namespace Contracts
{
    // Legacy PurchaseOrderDto removed (merged into PurchasingDtos.cs)

    public class CreatePurchaseOrderRequest
    {
        public int IdProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        // In the SP, it links to a Presupuesto, but for simplicity we might create the budget implicitly or just the order.
        // The SP sp_RegistrarOrdenCompra takes id_presupuesto. 
        // Let's assume we need to create a budget first or the UI handles it. 
        // For now, let's stick to the SP signature: sp_RegistrarOrdenCompra(@id_presupuesto, ...)
        // But wait, if the user just wants to create an Order, they might not have a budget ID yet.
        // Let's look at the flow. sp_RegistrarPresupuestoCompra returns nothing (ID?).
        // If the system requires a budget first, we should probably expose that.
        // However, to simplify for the user, maybe we wrap both?
        // Let's stick to the visible SPs.
        public int? IdPresupuesto { get; set; } 
    }

    public class PurchaseInvoiceDto
    {
        public int Id { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }

    public class CreatePurchaseInvoiceSimpleRequest
    {
        public int IdProveedor { get; set; }
        public int IdRemito { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public bool VisadoAlmacen { get; set; }
    }
}
