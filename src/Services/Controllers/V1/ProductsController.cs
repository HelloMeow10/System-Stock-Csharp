using Asp.Versioning;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers.V1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class ProductsController : BaseApiController
{
    private readonly IProductCatalogService _service;

    public ProductsController(IProductCatalogService service)
    {
        _service = service;
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
}
