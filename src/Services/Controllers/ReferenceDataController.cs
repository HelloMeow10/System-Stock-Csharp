using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Services.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class ReferenceDataController : BaseApiController
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        [HttpGet("tiposdoc")]
        [ProducesResponseType(typeof(IEnumerable<TipoDocDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TipoDocDto>>> GetTiposDoc()
        {
            var data = await _referenceDataService.GetTiposDocAsync();
            return Ok(data);
        }

        [HttpGet("provincias")]
        [ProducesResponseType(typeof(IEnumerable<ProvinciaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProvinciaDto>>> GetProvincias()
        {
            var data = await _referenceDataService.GetProvinciasAsync();
            return Ok(data);
        }

        [HttpGet("partidos/{provinciaId}")]
        [ProducesResponseType(typeof(IEnumerable<PartidoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PartidoDto>>> GetPartidos(int provinciaId)
        {
            var data = await _referenceDataService.GetPartidosByProvinciaIdAsync(provinciaId);
            return Ok(data);
        }

        [HttpGet("localidades/{partidoId}")]
        [ProducesResponseType(typeof(IEnumerable<LocalidadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LocalidadDto>>> GetLocalidades(int partidoId)
        {
            var data = await _referenceDataService.GetLocalidadesByPartidoIdAsync(partidoId);
            return Ok(data);
        }

        [HttpGet("generos")]
        [ProducesResponseType(typeof(IEnumerable<GeneroDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GeneroDto>>> GetGeneros()
        {
            var data = await _referenceDataService.GetGenerosAsync();
            return Ok(data);
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
        {
            var data = await _referenceDataService.GetRolesAsync();
            return Ok(data);
        }
    }
}