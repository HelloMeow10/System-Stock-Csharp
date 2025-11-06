using System.Net.Http.Json;
using Contracts;

namespace AgileStockPro.Client;

public class AgileStockProClient
{
    private readonly HttpClient _http;

    public AgileStockProClient(HttpClient http)
    {
        _http = http;
        if (_http.BaseAddress is null)
            throw new InvalidOperationException("HttpClient.BaseAddress must be set for AgileStockProClient");
    }

    // Catalog
    public async Task<(IEnumerable<ProductDto> Items, int TotalRecords, int PageNumber, int PageSize)> GetProductsAsync(int pageNumber = 1, int pageSize = 50, string? search = null, CancellationToken ct = default)
    {
        var url = $"api/v1/products?pageNumber={pageNumber}&pageSize={pageSize}" + (string.IsNullOrWhiteSpace(search) ? string.Empty : $"&search={Uri.EscapeDataString(search)}");
        var response = await _http.GetFromJsonAsync<PagedResponse<ProductDto>>(url, ct) ?? new();
        return (response.Items ?? Array.Empty<ProductDto>(), response.TotalRecords, response.PageNumber, response.PageSize);
    }

    public Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<ProductDto>($"api/v1/products/{id}", ct);

    public Task<IEnumerable<CategoryDto>?> GetCategoriesAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IEnumerable<CategoryDto>>("api/v1/categories", ct);

    public Task<IEnumerable<BrandDto>?> GetBrandsAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IEnumerable<BrandDto>>("api/v1/brands", ct);

    // Stock Reads
    public Task<IEnumerable<StockMovementDto>?> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IEnumerable<StockMovementDto>>($"api/v1/stock/movements?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}", ct);

    public Task<IEnumerable<ScrapReportItemDto>?> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IEnumerable<ScrapReportItemDto>>($"api/v1/stock/scrap?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}", ct);

    public Task<IEnumerable<ScrapReasonDto>?> GetScrapReasonsAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IEnumerable<ScrapReasonDto>>("api/v1/stock/reasons", ct);

    // Stock Writes
    public async Task IngresoAsync(IngresoMercaderiaRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/v1/stock/ingreso", request, ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/v1/stock/scrap", request, ct);
        resp.EnsureSuccessStatusCode();
    }

    public sealed class PagedResponse<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
