using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using System.Threading.Tasks;
using Services.Models;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Models;
using System.Collections.Generic;

namespace Services.Controllers
{
    [AllowAnonymous]
    public class ReferenceDataController : BaseApiController
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
            return Ok(ApiResponse<List<TipoDocDto>>.CreateSuccess(data));
        }

        [HttpGet("provincias")]
        public async Task<IActionResult> GetProvincias()
        {
            var data = await _referenceDataService.GetProvinciasAsync();
            return Ok(ApiResponse<List<ProvinciaDto>>.CreateSuccess(data));
        }

        [HttpGet("partidos/{provinciaId}")]
        public async Task<IActionResult> GetPartidos(int provinciaId)
        {
            var data = await _referenceDataService.GetPartidosByProvinciaAsync(provinciaId);
            return Ok(ApiResponse<List<PartidoDto>>.CreateSuccess(data));
        }

        [HttpGet("localidades/{partidoId}")]
        public async Task<IActionResult> GetLocalidades(int partidoId)
        {
            var data = await _referenceDataService.GetLocalidadesByPartidoAsync(partidoId);
            return Ok(ApiResponse<List<LocalidadDto>>.CreateSuccess(data));
        }

        [HttpGet("generos")]
        public async Task<IActionResult> GetGeneros()
        {
            var data = await _referenceDataService.GetGenerosAsync();
            return Ok(ApiResponse<List<GeneroDto>>.CreateSuccess(data));
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _referenceDataService.GetRolesAsync();
            return Ok(ApiResponse<List<RolDto>>.CreateSuccess(data));
        }
    }
}
