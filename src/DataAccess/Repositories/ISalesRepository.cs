using Contracts;
using Contracts;

namespace DataAccess.Repositories
{
    public interface ISalesRepository
    {
        Task<int> CreateSaleAsync(int clientId, DateTime date, string docType, decimal total, int statusId);
        Task AddSaleDetailAsync(int saleId, int productId, int quantity, decimal unitPrice);
        Task<IEnumerable<SaleOrderDto>> GetSalesAsync(DateTime? start, DateTime? end, int? clientId);
        Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? start, DateTime? end);
    }
}
