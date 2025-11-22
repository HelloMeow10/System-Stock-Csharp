using Asp.Versioning;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers.V1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class BrandsController : BaseApiController
{
    private readonly IProductCatalogService _service;
    public BrandsController(IProductCatalogService service) => _service = service;

    [HttpGet(Name = "GetBrandsV1")]
    [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<BrandDto>> GetAll(CancellationToken ct) => await _service.GetBrandsAsync(ct);
}
