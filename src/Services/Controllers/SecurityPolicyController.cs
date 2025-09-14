using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

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
            AddLinksToPolicy(politica);
            return Ok(politica);
        }

        private void AddLinksToPolicy(PoliticaSeguridadDto politica)
        {
            politica.Links.Add(CreateLink("GetSecurityPolicy", null, "self", "GET"));
            politica.Links.Add(CreateLink("UpdateSecurityPolicy", null, "update_policy", "PUT"));
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
