using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityPolicyController : ControllerBase
    {
        private readonly ISecurityPolicyService _securityPolicyService;

        public SecurityPolicyController(ISecurityPolicyService securityPolicyService)
        {
            _securityPolicyService = securityPolicyService;
        }

        [HttpGet]
        public ActionResult<PoliticaSeguridadDto> Get()
        {
            var politica = _securityPolicyService.GetPoliticaSeguridad();
            return Ok(politica);
        }

        [HttpPut]
        public IActionResult Put([FromBody] PoliticaSeguridadDto politica)
        {
            _securityPolicyService.UpdatePoliticaSeguridad(politica);
            return NoContent();
        }
    }
}
