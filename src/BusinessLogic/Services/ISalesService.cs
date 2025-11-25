using Contracts;

namespace BusinessLogic.Services
{
    public interface ISalesService
    {
        Task CreateSaleAsync(CreateSaleOrderRequest request);
        Task<IEnumerable<SaleOrderDto>> GetSalesAsync(DateTime? start, DateTime? end, int? clientId);
        Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? start, DateTime? end);
    }
}
