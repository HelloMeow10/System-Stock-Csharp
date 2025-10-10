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
        public async Task<IEnumerable<TipoDocDto>> GetTiposDoc()
        {
            return await _referenceDataService.GetTiposDocAsync();
        }

        [HttpGet("provincias")]
        [ProducesResponseType(typeof(IEnumerable<ProvinciaDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ProvinciaDto>> GetProvincias()
        {
            return await _referenceDataService.GetProvinciasAsync();
        }

        [HttpGet("partidos/{provinciaId}")]
        [ProducesResponseType(typeof(IEnumerable<PartidoDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PartidoDto>> GetPartidos(int provinciaId)
        {
            return await _referenceDataService.GetPartidosByProvinciaIdAsync(provinciaId);
        }

        [HttpGet("localidades/{partidoId}")]
        [ProducesResponseType(typeof(IEnumerable<LocalidadDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<LocalidadDto>> GetLocalidades(int partidoId)
        {
            return await _referenceDataService.GetLocalidadesByPartidoIdAsync(partidoId);
        }

        [HttpGet("generos")]
        [ProducesResponseType(typeof(IEnumerable<GeneroDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GeneroDto>> GetGeneros()
        {
            return await _referenceDataService.GetGenerosAsync();
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<RolDto>> GetRoles()
        {
            return await _referenceDataService.GetRolesAsync();
        }
    }
}