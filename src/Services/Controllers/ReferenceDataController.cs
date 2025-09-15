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
        public ActionResult<IEnumerable<TipoDocDto>> GetTiposDoc()
        {
            return _referenceDataService.GetTiposDoc();
        }

        [HttpGet("provincias")]
        [ProducesResponseType(typeof(IEnumerable<ProvinciaDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProvinciaDto>> GetProvincias()
        {
            return _referenceDataService.GetProvincias();
        }

        [HttpGet("partidos/{provinciaId}")]
        [ProducesResponseType(typeof(IEnumerable<PartidoDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PartidoDto>> GetPartidos(int provinciaId)
        {
            return _referenceDataService.GetPartidosByProvinciaId(provinciaId);
        }

        [HttpGet("localidades/{partidoId}")]
        [ProducesResponseType(typeof(IEnumerable<LocalidadDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LocalidadDto>> GetLocalidades(int partidoId)
        {
            return _referenceDataService.GetLocalidadesByPartidoId(partidoId);
        }

        [HttpGet("generos")]
        [ProducesResponseType(typeof(IEnumerable<GeneroDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneroDto>> GetGeneros()
        {
            return _referenceDataService.GetGeneros();
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RolDto>> GetRoles()
        {
            return _referenceDataService.GetRoles();
        }
    }
}
