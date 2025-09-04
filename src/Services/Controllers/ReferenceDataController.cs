using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using System.Threading.Tasks;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        [HttpGet("tiposdoc")]
        public async Task<IActionResult> GetTiposDoc()
        {
            var data = await _referenceDataService.GetTiposDocAsync();
            return Ok(data);
        }

        [HttpGet("provincias")]
        public async Task<IActionResult> GetProvincias()
        {
            var data = await _referenceDataService.GetProvinciasAsync();
            return Ok(data);
        }

        [HttpGet("partidos/{provinciaId}")]
        public async Task<IActionResult> GetPartidos(int provinciaId)
        {
            var data = await _referenceDataService.GetPartidosByProvinciaAsync(provinciaId);
            return Ok(data);
        }

        [HttpGet("localidades/{partidoId}")]
        public async Task<IActionResult> GetLocalidades(int partidoId)
        {
            var data = await _referenceDataService.GetLocalidadesByPartidoAsync(partidoId);
            return Ok(data);
        }

        [HttpGet("generos")]
        public async Task<IActionResult> GetGeneros()
        {
            var data = await _referenceDataService.GetGenerosAsync();
            return Ok(data);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _referenceDataService.GetRolesAsync();
            return Ok(data);
        }
    }
}
