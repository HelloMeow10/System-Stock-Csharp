using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services;

public interface IPurchasingService
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
    Task ReceiveOrderAsync(int orderId, IEnumerable<IngresoMercaderiaRequest> items);
    Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(DateTime? start, DateTime? end);
}

public class PurchasingService : IPurchasingService
{
    private readonly IPurchasingRepository _repo;
    private readonly IStockService _stockService;

    public PurchasingService(IPurchasingRepository repo, IStockService stockService)
    {
        _repo = repo;
        _stockService = stockService;
    }

    public Task<int> CreateQuoteAsync(int proveedorId, DateTime fecha) => _repo.CreateQuoteAsync(proveedorId, fecha);
    public Task AddQuoteItemAsync(int quoteId, int productoId, int cantidad, decimal precioUnitario) => _repo.AddQuoteItemAsync(quoteId, productoId, cantidad, precioUnitario);
    public Task<IEnumerable<PurchaseQuoteDto>> ListQuotesAsync(int? proveedorId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null) => _repo.ListQuotesAsync(proveedorId, fechaDesde, fechaHasta);
    public Task<int> ConvertQuoteToOrderAsync(int quoteId, DateTime? fecha = null) => _repo.ConvertQuoteToOrderAsync(quoteId, fecha);
    public Task<IEnumerable<PurchaseOrderDto>> ListOrdersAsync(int? proveedorId = null, bool? entregado = null) => _repo.ListOrdersAsync(proveedorId, entregado);
    public Task AddOrderItemAsync(int orderId, int productoId, int cantidad, decimal precioUnitario) => _repo.AddOrderItemAsync(orderId, productoId, cantidad, precioUnitario);
    public Task MarkOrderReceivedAsync(int orderId) => _repo.MarkOrderReceivedAsync(orderId);
    public Task<IEnumerable<PurchaseQuoteItemDto>> GetQuoteItemsAsync(int quoteId) => _repo.GetQuoteItemsAsync(quoteId);
    public Task<IEnumerable<PurchaseOrderItemDto>> GetOrderItemsAsync(int orderId) => _repo.GetOrderItemsAsync(orderId);
    public Task DeleteQuoteItemAsync(int itemId) => _repo.DeleteQuoteItemAsync(itemId);
    public Task UpdateQuoteItemAsync(int itemId, int cantidad, decimal precioUnitario) => _repo.UpdateQuoteItemAsync(itemId, cantidad, precioUnitario);
    public Task DeleteOrderItemAsync(int itemId) => _repo.DeleteOrderItemAsync(itemId);
    public Task UpdateOrderItemAsync(int itemId, int cantidad, decimal precioUnitario) => _repo.UpdateOrderItemAsync(itemId, cantidad, precioUnitario);

    public Task<PurchaseOrderDto?> GetOrderByIdAsync(int id) => _repo.GetOrderByIdAsync(id);

    public async Task ReceiveOrderAsync(int orderId, IEnumerable<IngresoMercaderiaRequest> items)
    {
        // 1. Update Stock for each item
        foreach (var item in items)
        {
            await _stockService.IngresoMercaderiaAsync(item);
        }

        // 2. Mark Order as Received
        await _repo.MarkOrderReceivedAsync(orderId);
    }

    public Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(DateTime? start, DateTime? end) => _repo.GetPurchaseReportAsync(start, end);
}
