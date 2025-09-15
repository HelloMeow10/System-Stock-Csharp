using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Hateoas;
using BusinessLogic.Exceptions;

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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var politica = await _securityPolicyService.GetPoliticaSeguridadAsync();
            if (politica == null)
            {
                throw new BusinessLogicException("Security policy not found.");
            }
            return Ok(politica);
        }

        [HttpPut(Name = "UpdateSecurityPolicy")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            await _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
            return Ok(new { message = "Security policy updated successfully." });
        }
    }
}
