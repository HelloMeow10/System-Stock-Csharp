using Contracts;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _repository;
        private readonly IProductCatalogService _productService;

        public SalesService(ISalesRepository repository, IProductCatalogService productService)
        {
            _repository = repository;
            _productService = productService;
        }

        public async Task CreateSaleAsync(CreateSaleOrderRequest request)
        {
            // 0. Validate Stock
            foreach (var item in request.Items)
            {
                var product = await _productService.GetProductByIdAsync(item.IdProducto);
                if (product == null)
                    throw new InvalidOperationException($"Producto con ID {item.IdProducto} no encontrado.");

                if (product.StockActual < item.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para el producto '{product.Nombre}'. Solicitado: {item.Cantidad}, Disponible: {product.StockActual}");
            }

            // 1. Create Sale Header
            // Default status 1 (assuming 1 is 'Pending' or similar in EstadoVentas)
            // Estado: resolver desde BD el primero disponible (evitar IDs fijos)
            var statusId = await _repository.GetDefaultSalesStatusIdAsync();
            int saleId = await _repository.CreateSaleAsync(request.IdCliente, request.Fecha, request.TipoDocumento, request.Total, statusId);

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

        public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? start, DateTime? end)
        {
            return await _repository.GetSalesReportAsync(start, end);
        }
    }
}
