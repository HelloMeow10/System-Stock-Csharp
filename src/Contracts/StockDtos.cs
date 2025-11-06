namespace Contracts;

public record ProductDto
(
    int Id,
    string Codigo,
    string Nombre,
    string Categoria,
    string Marca,
    decimal Precio,
    int StockActual,
    int StockMinimo,
    int StockMaximo
);

public record CategoryDto(int Id, string Nombre);

public record BrandDto(int Id, string Nombre);

// Stock movements and scrap
public record StockMovementDto
(
    int Id,
    DateTime Fecha,
    string Usuario,
    string Producto,
    string TipoMovimiento,
    int Cantidad
);

public record ScrapReportItemDto
(
    int Id,
    DateTime Fecha,
    string Usuario,
    string Producto,
    string Motivo,
    int Cantidad
);

public record ScrapReasonDto
(
    int Id,
    bool Dano,
    bool Vencido,
    bool Obsoleto,
    bool MalaCalidad
);

public record CreateScrapRequest
(
    int ProductoId,
    int Cantidad,
    int UsuarioId,
    int MotivoScrapId
);

public record IngresoMercaderiaRequest
(
    int ProductoId,
    int UsuarioId,
    int Cantidad,
    string? Lote,
    int? StockMinimo,
    int? StockIdeal,
    int? StockMaximo,
    string? TipoStock,
    int? PuntoReposicion,
    DateOnly? FechaVencimiento,
    string? EstadoHabilitaciones,
    int? MovimientoStockId
);

