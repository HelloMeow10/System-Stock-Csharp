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
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string CondicionIva { get; set; } = string.Empty;
        public int PlazoPagoDias { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public List<SupplierContactDto> Contactos { get; set; } = new();
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
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string CondicionIva { get; set; } = string.Empty;
        public int PlazoPagoDias { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public List<SupplierContactDto> Contactos { get; set; } = new();
    }

    public class UpdateSupplierRequest : CreateSupplierRequest
    {
        public int Id { get; set; }
    }

    public class SupplierContactDto
    {
        public int? Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
