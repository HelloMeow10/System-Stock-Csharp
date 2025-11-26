using Contracts;

namespace DataAccess.Repositories;

public interface IPurchasingRepository
{
    Task<int> CreateQuoteAsync(int proveedorId, DateTime fecha);
    Task AddQuoteItemAsync(int quoteId, int productoId, int cantidad, decimal precioUnitario);
    Task<IEnumerable<PurchaseQuoteDto>> ListQuotesAsync(int? proveedorId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null);
    Task<int> ConvertQuoteToOrderAsync(int quoteId, DateTime? fecha = null);
    Task<IEnumerable<PurchaseOrderDto>> ListOrdersAsync(int? proveedorId = null, bool? entregado = null);
    Task AddOrderItemAsync(int orderId, int productoId, int cantidad, decimal precioUnitario);
    Task MarkOrderReceivedAsync(int orderId);
    Task<IEnumerable<PurchaseQuoteItemDto>> GetQuoteItemsAsync(int quoteId);
    Task<IEnumerable<PurchaseOrderItemDto>> GetOrderItemsAsync(int orderId);
    Task DeleteQuoteItemAsync(int itemId);
    Task UpdateQuoteItemAsync(int itemId, int cantidad, decimal precioUnitario);
    Task DeleteOrderItemAsync(int itemId);
    Task UpdateOrderItemAsync(int itemId, int cantidad, decimal precioUnitario);
    Task<PurchaseOrderDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(DateTime? start, DateTime? end);
}
