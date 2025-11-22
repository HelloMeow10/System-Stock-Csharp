using Contracts;

namespace DataAccess.Repositories
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<PurchaseOrderDto>> GetOrdersAsync(bool? delivered = null);
        Task<int> CreateBudgetAsync(int supplierId, DateTime date, decimal total);
        Task CreateOrderAsync(int budgetId, DateTime date, decimal total);
        Task CreateRemitoAsync(int orderId, string remitoNumber, DateTime date, bool hasInvoice);
        Task CreateInvoiceAsync(CreatePurchaseInvoiceRequest request);
        Task<IEnumerable<PurchaseInvoiceDto>> GetInvoicesAsync(DateTime? start, DateTime? end, int? supplierId, int? productId);
    }
}
