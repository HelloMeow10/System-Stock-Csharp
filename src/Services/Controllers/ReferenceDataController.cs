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
        [ProducesResponseType(typeof(IEnumerable<TipoDocDto>), StatusCodes.Status200OK)]
        public IActionResult GetTiposDoc()
        {
            var data = _referenceDataService.GetTiposDoc();
            return Ok(data);
        }

        [HttpGet("provincias")]
        [ProducesResponseType(typeof(IEnumerable<ProvinciaDto>), StatusCodes.Status200OK)]
        public IActionResult GetProvincias()
        {
            var data = _referenceDataService.GetProvincias();
            return Ok(data);
        }

        [HttpGet("partidos/{provinciaId}")]
        [ProducesResponseType(typeof(IEnumerable<PartidoDto>), StatusCodes.Status200OK)]
        public IActionResult GetPartidos(int provinciaId)
        {
            var data = _referenceDataService.GetPartidosByProvinciaId(provinciaId);
            return Ok(data);
        }

        [HttpGet("localidades/{partidoId}")]
        [ProducesResponseType(typeof(IEnumerable<LocalidadDto>), StatusCodes.Status200OK)]
        public IActionResult GetLocalidades(int partidoId)
        {
            var data = _referenceDataService.GetLocalidadesByPartidoId(partidoId);
            return Ok(data);
        }

        [HttpGet("generos")]
        [ProducesResponseType(typeof(IEnumerable<GeneroDto>), StatusCodes.Status200OK)]
        public IActionResult GetGeneros()
        {
            var data = _referenceDataService.GetGeneros();
            return Ok(data);
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public IActionResult GetRoles()
        {
            var data = _referenceDataService.GetRoles();
            return Ok(data);
        }
    }
}
