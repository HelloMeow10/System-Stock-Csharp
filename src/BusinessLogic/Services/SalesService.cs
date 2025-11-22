using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _repository;

        public SalesService(ISalesRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateSaleAsync(CreateSaleOrderRequest request)
        {
            // 1. Create Sale Header
            // Default status 1 (assuming 1 is 'Pending' or similar in EstadoVentas)
            int saleId = await _repository.CreateSaleAsync(request.IdCliente, request.Fecha, request.TipoDocumento, request.Total, 1);

            // 2. Create Details
            foreach (var item in request.Items)
            {
                await _repository.AddSaleDetailAsync(saleId, item.IdProducto, item.Cantidad, item.PrecioUnitario);
            }
        }

        public async Task<IEnumerable<SaleOrderDto>> GetSalesAsync(DateTime? start, DateTime? end, int? clientId)
        {
            return await _repository.GetSalesAsync(start, end, clientId);
        }
    }
}
