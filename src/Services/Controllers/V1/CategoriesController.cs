using Asp.Versioning;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers.V1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class CategoriesController : BaseApiController
{
    private readonly IProductCatalogService _service;
    public CategoriesController(IProductCatalogService service) => _service = service;

    [HttpGet(Name = "GetCategoriesV1")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<CategoryDto>> GetAll(CancellationToken ct) => await _service.GetCategoriesAsync(ct);
}
