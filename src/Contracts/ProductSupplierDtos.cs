namespace Contracts
{
    public class ProductSupplierRelationDto
    {
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? TiempoEntrega { get; set; }
        public decimal? Descuento { get; set; }
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoNombre { get; set; } = string.Empty;
        public string ProveedorCodigo { get; set; } = string.Empty;
        public string ProveedorNombre { get; set; } = string.Empty;
        public string ProveedorRazonSocial { get; set; } = string.Empty;
    }
}
