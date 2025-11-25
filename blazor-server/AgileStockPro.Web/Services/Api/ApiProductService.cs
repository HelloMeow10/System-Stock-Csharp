using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services.Api;

public class ApiProductService : IProductService
{
    private readonly BackendApiClient _api;
    public ApiProductService(BackendApiClient api) => _api = api;

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        // Fetch first 200 products; later we can add paging/search UI.
        var resp = await _api.GetAsync<PagedResponse<ProductDto>>("api/v1/products?pageNumber=1&pageSize=200");
        var items = resp.Items ?? Enumerable.Empty<ProductDto>();
        return items.Select(Map);
    }

    public Task CreateAsync(Contracts.CreateProductRequest req)
        => _api.PostAsync("api/v1/products", req);

    private static Product Map(ProductDto dto)
    {
        var status = "ok";
        if (dto.StockActual < Math.Max(1, dto.StockMinimo / 2)) status = "critical";
        else if (dto.StockActual < dto.StockMinimo) status = "low";

        return new Product
        {
            DbId = dto.Id,
            Id = dto.Codigo,
            Name = dto.Nombre,
            Category = dto.Categoria,
            Brand = dto.Marca,
            Stock = dto.StockActual,
            MinStock = dto.StockMinimo,
            MaxStock = dto.StockMaximo,
            Status = status,
            UnidadMedida = dto.UnidadMedida,
            Peso = dto.Peso,
            Volumen = dto.Volumen,
            PuntoReposicion = dto.PuntoReposicion,
            DiasVencimiento = dto.DiasVencimiento,
            LoteObligatorio = dto.LoteObligatorio,
            ControlVencimiento = dto.ControlVencimiento
        };
    }

    // Backend DTOs
    public class ProductDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public decimal Volumen { get; set; }
        public int PuntoReposicion { get; set; }
        public int DiasVencimiento { get; set; }
        public bool LoteObligatorio { get; set; }
        public bool ControlVencimiento { get; set; }
    }
}
