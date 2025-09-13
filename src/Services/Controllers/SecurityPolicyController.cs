using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecurityPolicyController : BaseApiController
    {
        private readonly ISecurityPolicyService _securityPolicyService;

        public SecurityPolicyController(ISecurityPolicyService securityPolicyService)
        {
            _securityPolicyService = securityPolicyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PoliticaSeguridadDto> Get()
        {
            var politica = _securityPolicyService.GetPoliticaSeguridad();
            if (politica == null)
            {
                return NotFound();
            }
            return Ok(politica);
        }

        [HttpPut]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] PoliticaSeguridadDto politica)
        {
            var updatedPolitica = _securityPolicyService.UpdatePoliticaSeguridad(politica);
            return Ok(updatedPolitica);
        }
    }
}
