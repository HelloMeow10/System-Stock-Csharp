namespace Contracts
{
    public class StockItemDto
    {
        public int Id { get; set; }
        public string Product { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty; // Ubicacion
        public int Quantity { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public record StockMovementDto(
        int Id,
        DateTime Fecha,
        string Usuario,
        string Producto,
        string TipoMovimiento,
        int Cantidad
    );

    public record ScrapReportItemDto(
        int Id,
        DateTime Fecha,
        string Usuario,
        string Producto,
        string Motivo,
        int Cantidad
    );

    public record ScrapReasonDto(
        int Id,
        bool Dano,
        bool Vencido,
        bool Obsoleto,
        bool MalaCalidad
    );

    public class IngresoMercaderiaRequest
    {
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public string? Lote { get; set; }
        public int Cantidad { get; set; }
        public int? StockMinimo { get; set; }
        public int? StockIdeal { get; set; }
        public int? StockMaximo { get; set; }
        public string? TipoStock { get; set; }
        public int? PuntoReposicion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? EstadoHabilitaciones { get; set; }
        public int? MovimientoStockId { get; set; }
    }

    public class CreateScrapRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public int UsuarioId { get; set; }
        public int MotivoScrapId { get; set; }
    }

    public record StockAlertDto(
        string Codigo,
        string Producto,
        int StockActual,
        int StockMinimo,
        int PuntoReposicion,
        string TipoAlerta,
        string Severidad,
        string? Lote = null,
        DateTime? FechaVencimiento = null
    );
}
