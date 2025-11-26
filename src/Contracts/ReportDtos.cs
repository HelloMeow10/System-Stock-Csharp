namespace Contracts
{
    public record SalesReportDto(
        int IdVenta,
        DateTime Fecha,
        string Cliente,
        string Producto,
        string Categoria,
        int Cantidad,
        decimal PrecioUnitario,
        decimal Subtotal,
        decimal MontoTotalVenta
    );

    public record PurchaseReportDto(
        int IdOrden,
        DateTime Fecha,
        string Proveedor,
        string Producto,
        int Cantidad,
        decimal PrecioUnitario,
        decimal Subtotal,
        bool Entregado
    );

    public record StockValuationDto(
        string Codigo,
        string Producto,
        string Categoria,
        int StockActual,
        decimal PrecioCompra,
        decimal ValorTotal
    );
}
