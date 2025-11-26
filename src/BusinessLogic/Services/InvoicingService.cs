using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class InvoicingService : IInvoicingService
    {
        private readonly IInvoicingRepository _repo;

        public InvoicingService(IInvoicingRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<InvoiceDto>> GetPurchaseInvoicesAsync(DateTime? from, DateTime? to, int? supplierId, CancellationToken ct = default)
            => _repo.GetPurchaseInvoicesAsync(from, to, supplierId, ct);

        public Task CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request, CancellationToken ct = default)
            => _repo.CreatePurchaseInvoiceAsync(request, ct);

        public Task<IEnumerable<InvoiceDto>> GetSalesInvoicesAsync(DateTime? from, DateTime? to, int? customerId, CancellationToken ct = default)
            => _repo.GetSalesInvoicesAsync(from, to, customerId, ct);

        public Task CreateSalesInvoiceAsync(CreateSalesInvoiceRequest request, CancellationToken ct = default)
            => _repo.CreateSalesInvoiceAsync(request, ct);
    }
}
