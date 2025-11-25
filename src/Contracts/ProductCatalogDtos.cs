namespace Contracts
{
    public record BrandDto(int Id, string Nombre);
    public record CategoryDto(int Id, string Nombre);
    public record ProductDto(
        int Id,
        string Codigo,
        string Nombre,
        string Categoria,
        string Marca,
        decimal Precio,
        int StockActual,
        int StockMinimo,
        int StockMaximo,
        string UnidadMedida,
        decimal Peso,
        decimal Volumen,
        int PuntoReposicion,
        int DiasVencimiento,
        bool LoteObligatorio,
        bool ControlVencimiento
    );
}
