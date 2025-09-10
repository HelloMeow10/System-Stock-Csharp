using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
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
        public ActionResult<IEnumerable<TipoDocDto>> GetTiposDoc()
        {
            var data = _referenceDataService.GetTiposDoc();
            return Ok(data);
        }

        [HttpGet("provincias")]
        public ActionResult<IEnumerable<ProvinciaDto>> GetProvincias()
        {
            var data = _referenceDataService.GetProvincias();
            return Ok(data);
        }

        [HttpGet("partidos/{provinciaId}")]
        public ActionResult<IEnumerable<PartidoDto>> GetPartidos(int provinciaId)
        {
            var data = _referenceDataService.GetPartidosByProvinciaId(provinciaId);
            return Ok(data);
        }

        [HttpGet("localidades/{partidoId}")]
        public ActionResult<IEnumerable<LocalidadDto>> GetLocalidades(int partidoId)
        {
            var data = _referenceDataService.GetLocalidadesByPartidoId(partidoId);
            return Ok(data);
        }

        [HttpGet("generos")]
        public ActionResult<IEnumerable<GeneroDto>> GetGeneros()
        {
            var data = _referenceDataService.GetGeneros();
            return Ok(data);
        }

        [HttpGet("roles")]
        public ActionResult<IEnumerable<RolDto>> GetRoles()
        {
            var data = _referenceDataService.GetRoles();
            return Ok(data);
        }
    }
}
