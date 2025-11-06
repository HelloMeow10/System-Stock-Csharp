using Asp.Versioning;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers.V1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class StockController : BaseApiController
{
    private readonly IStockService _service;
    public StockController(IStockService service) => _service = service;

    [HttpGet("movements", Name = "GetStockMovementsV1")]
    [ProducesResponseType(typeof(IEnumerable<StockMovementDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<StockMovementDto>> GetMovements([FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct)
        => await _service.GetMovementsAsync(from, to, ct);

    [HttpPost("ingreso", Name = "PostIngresoMercaderiaV1")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Ingreso([FromBody] IngresoMercaderiaRequest request, CancellationToken ct)
    {
        await _service.IngresoMercaderiaAsync(request, ct);
        return NoContent();
    }

    [HttpPost("scrap", Name = "PostScrapV1")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Scrap([FromBody] CreateScrapRequest request, CancellationToken ct)
    {
        await _service.CreateScrapAsync(request, ct);
        return NoContent();
    }

    [HttpGet("scrap", Name = "GetScrapReportV1")]
    [ProducesResponseType(typeof(IEnumerable<ScrapReportItemDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<ScrapReportItemDto>> GetScrap([FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct)
        => await _service.GetScrapAsync(from, to, ct);

    [HttpGet("reasons", Name = "GetScrapReasonsV1")]
    [ProducesResponseType(typeof(IEnumerable<ScrapReasonDto>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<ScrapReasonDto>> GetReasons(CancellationToken ct)
        => await _service.GetScrapReasonsAsync(ct);
}
