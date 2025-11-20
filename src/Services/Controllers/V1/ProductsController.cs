using Asp.Versioning;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Services.Controllers.V1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class ProductsController : BaseApiController
{
    private readonly IProductCatalogService _service;
    private readonly Microsoft.AspNetCore.SignalR.IHubContext<Services.Hubs.NotificationHub> _hubContext;

    public ProductsController(IProductCatalogService service, Microsoft.AspNetCore.SignalR.IHubContext<Services.Hubs.NotificationHub> hubContext)
    {
        _service = service;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Gets a paged list of products with optional search by code, name, brand or category.
    /// </summary>
    [HttpGet(Name = "GetProductsV1")]
    [ProducesResponseType(typeof(PagedResponse<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ProductDto>>> GetProducts([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _service.GetProductsAsync(search, pageNumber, pageSize, ct);
        return Ok(result);
    }

    /// <summary>
    /// Gets a single product by id.
    /// </summary>
    [HttpGet("{id:int}", Name = "GetProductByIdV1")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct = default)
    {
        var prod = await _service.GetProductByIdAsync(id, ct);
        if (prod is null) return NotFound();
        return Ok(prod);
    }

    [HttpPost(Name = "CreateProductV1")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken ct = default)
    {
        var created = await _service.CreateProductAsync(request, ct);

        // Broadcast to connected desktop/web clients
        try
        {
            await _hubContext.Clients.All.SendAsync("ProductCreated", created, ct);
        }
        catch
        {
            // Non-fatal: continue even if broadcasting fails
        }

        return CreatedAtRoute("GetProductByIdV1", new { id = created.Id }, created);
    }
}
