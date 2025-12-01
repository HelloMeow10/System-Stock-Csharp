using Contracts;

namespace DataAccess.Repositories
{
    public interface IInvoicingRepository
    {
        Task<IEnumerable<InvoiceDto>> GetPurchaseInvoicesAsync(DateTime? from, DateTime? to, int? supplierId, CancellationToken ct = default);
        Task CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request, CancellationToken ct = default);
        
        Task<IEnumerable<InvoiceDto>> GetSalesInvoicesAsync(DateTime? from, DateTime? to, int? customerId, CancellationToken ct = default);
        Task CreateSalesInvoiceAsync(CreateSalesInvoiceRequest request, CancellationToken ct = default);
        Task<IEnumerable<PaymentMethodDto>> GetPaymentMethodsAsync(CancellationToken ct = default);
    }
}
