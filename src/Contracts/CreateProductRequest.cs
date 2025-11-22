namespace Contracts;

public record CreateProductRequest(
    string Codigo,
    string? CodBarras,
    string Nombre,
    string? Descripcion,
    int? IdMarca,
    decimal? PrecioCompra,
    decimal? PrecioVenta,
    string? Estado,
    string? Ubicacion,
    bool? Habilitado,
    int? IdCategoria
);
