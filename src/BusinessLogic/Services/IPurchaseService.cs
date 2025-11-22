using Contracts;

namespace BusinessLogic.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseOrderDto>> GetOrdersAsync(bool? delivered = null);
        Task CreateOrderAsync(CreatePurchaseOrderRequest request);
        Task CreateInvoiceAsync(CreatePurchaseInvoiceRequest request);
        Task<IEnumerable<PurchaseInvoiceDto>> GetInvoicesAsync(DateTime? start, DateTime? end, int? supplierId, int? productId);
    }
}
