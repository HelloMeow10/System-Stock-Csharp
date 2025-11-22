namespace Contracts
{
    public class SaleOrderDto
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class CreateSaleOrderRequest
    {
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoDocumento { get; set; } = "NotaPedido"; // Default
        public decimal Total { get; set; }
        public List<SaleOrderDetailRequest> Items { get; set; } = new();
    }

    public class SaleOrderDetailRequest
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class SaleInvoiceDto
    {
        // Reuse SaleOrderDto or similar structure if the SP returns similar data
        // sp_ReporteVentas returns id_venta, cliente, fecha, tipoDocumento, numeroDocumento, montoTotal, estado
    }
}
