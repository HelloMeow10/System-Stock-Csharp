namespace Contracts
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public int TiempoEntrega { get; set; }
        public decimal Descuento { get; set; }
        public int IdFormaPago { get; set; }
        public string FormaPago { get; set; } = string.Empty;
    }

    public class CreateSupplierRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public int TiempoEntrega { get; set; }
        public decimal Descuento { get; set; }
        public int IdFormaPago { get; set; }
    }

    public class UpdateSupplierRequest : CreateSupplierRequest
    {
        public int Id { get; set; }
    }
}
