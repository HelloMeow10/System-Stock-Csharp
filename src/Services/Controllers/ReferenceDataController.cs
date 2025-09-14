using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TipoDocDto>>), StatusCodes.Status200OK)]
        public IActionResult GetTiposDoc()
        {
            var data = _referenceDataService.GetTiposDoc();
            return Ok(ApiResponse<IEnumerable<TipoDocDto>>.Success(data));
        }

        [HttpGet("provincias")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProvinciaDto>>), StatusCodes.Status200OK)]
        public IActionResult GetProvincias()
        {
            var data = _referenceDataService.GetProvincias();
            return Ok(ApiResponse<IEnumerable<ProvinciaDto>>.Success(data));
        }

        [HttpGet("partidos/{provinciaId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PartidoDto>>), StatusCodes.Status200OK)]
        public IActionResult GetPartidos(int provinciaId)
        {
            var data = _referenceDataService.GetPartidosByProvinciaId(provinciaId);
            return Ok(ApiResponse<IEnumerable<PartidoDto>>.Success(data));
        }

        [HttpGet("localidades/{partidoId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LocalidadDto>>), StatusCodes.Status200OK)]
        public IActionResult GetLocalidades(int partidoId)
        {
            var data = _referenceDataService.GetLocalidadesByPartidoId(partidoId);
            return Ok(ApiResponse<IEnumerable<LocalidadDto>>.Success(data));
        }

        [HttpGet("generos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GeneroDto>>), StatusCodes.Status200OK)]
        public IActionResult GetGeneros()
        {
            var data = _referenceDataService.GetGeneros();
            return Ok(ApiResponse<IEnumerable<GeneroDto>>.Success(data));
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDto>>), StatusCodes.Status200OK)]
        public IActionResult GetRoles()
        {
            var data = _referenceDataService.GetRoles();
            return Ok(ApiResponse<IEnumerable<RolDto>>.Success(data));
        }
    }
}
