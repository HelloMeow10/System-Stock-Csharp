using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;

        public PurchaseService(IPurchaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetOrdersAsync(bool? delivered = null)
        {
            return await _repository.GetOrdersAsync(delivered);
        }

        public async Task CreateOrderAsync(CreatePurchaseOrderRequest request)
        {
            // Logic: 
            // 1. Create Budget (Presupuesto)
            // 2. Create Order (OrdenCompra) linked to Budget
            
            // If IdPresupuesto is provided, use it. Otherwise create one.
            int budgetId = request.IdPresupuesto ?? 0;
            
            if (budgetId == 0)
            {
                budgetId = await _repository.CreateBudgetAsync(request.IdProveedor, request.Fecha, request.Total);
            }

            await _repository.CreateOrderAsync(budgetId, request.Fecha, request.Total);
        }

        public async Task CreateInvoiceAsync(CreatePurchaseInvoiceSimpleRequest request)
        {
            await _repository.CreateInvoiceAsync(request);
        }

        public async Task<IEnumerable<PurchaseInvoiceDto>> GetInvoicesAsync(DateTime? start, DateTime? end, int? supplierId, int? productId)
        {
            return await _repository.GetInvoicesAsync(start, end, supplierId, productId);
        }
    }
}
