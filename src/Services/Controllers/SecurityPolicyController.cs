using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Services.Hateoas;
using System.Threading.Tasks;

namespace Services.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecurityPolicyController : BaseApiController
    {
        private readonly ISecurityPolicyService _securityPolicyService;
        private readonly ILinkService _linkService;

        public SecurityPolicyController(ISecurityPolicyService securityPolicyService, ILinkService linkService)
        {
            _securityPolicyService = securityPolicyService;
            _linkService = linkService;
        }

        [HttpGet(Name = "GetSecurityPolicy")]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PoliticaSeguridadDto>> Get()
        {
            var politica = await _securityPolicyService.GetPoliticaSeguridadAsync();
            if (politica == null)
            {
                return NotFound();
            }
            _linkService.AddLinksForSecurityPolicy(Url, politica);
            return Ok(politica);
        }

        [HttpPut(Name = "UpdateSecurityPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            await _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
            return NoContent();
        }
    }
}
