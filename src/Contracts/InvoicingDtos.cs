namespace Contracts
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // Compra / Venta
        public string TipoDocumento { get; set; } = string.Empty; // Factura A, B, C, Nota Credito, etc.
        public string Numero { get; set; } = string.Empty; // Puntos de venta + numero
        public DateTime Fecha { get; set; }
        public string EntidadNombre { get; set; } = string.Empty; // Proveedor o Cliente
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class CreatePurchaseInvoiceRequest
    {
        public int ProveedorId { get; set; }
        public string TipoDocumento { get; set; } = "Factura A";
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public List<InvoiceDetailRequest> Items { get; set; } = new();
        public int? OrdenCompraId { get; set; } // Optional link to PO
    }

    public class CreateSalesInvoiceRequest
    {
        public int ClienteId { get; set; }
        public string TipoDocumento { get; set; } = "Factura B";
        public DateTime Fecha { get; set; }
        public List<InvoiceDetailRequest> Items { get; set; } = new();
        public int? NotaPedidoId { get; set; } // Optional link to Sales Order
    }

    public class InvoiceDetailRequest
    {
        public int ProductoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal AlicuotaIva { get; set; } // 21, 10.5, etc.
    }
}
