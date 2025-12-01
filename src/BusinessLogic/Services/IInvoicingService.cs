using Contracts;

namespace BusinessLogic.Services
{
    public interface IInvoicingService
    {
        // Purchase Invoicing
        Task<IEnumerable<InvoiceDto>> GetPurchaseInvoicesAsync(DateTime? from, DateTime? to, int? supplierId, CancellationToken ct = default);
        Task CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request, CancellationToken ct = default);
        
        // Sales Invoicing
        Task<IEnumerable<InvoiceDto>> GetSalesInvoicesAsync(DateTime? from, DateTime? to, int? customerId, CancellationToken ct = default);
        Task CreateSalesInvoiceAsync(CreateSalesInvoiceRequest request, CancellationToken ct = default);
        Task<IEnumerable<PaymentMethodDto>> GetPaymentMethodsAsync(CancellationToken ct = default);
    }
}
