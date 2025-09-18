using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Hateoas;
using BusinessLogic.Exceptions;
using Services.Authentication;
using Asp.Versioning;

namespace Services.Controllers
{
    [HasApiKey]
    [ApiVersion("1.0")]
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
        public async Task<ActionResult<PoliticaSeguridadDto>> Get()
        {
            var policy = await _securityPolicyService.GetPoliticaSeguridadAsync();
            return policy;
        }

        [HttpPut(Name = "UpdateSecurityPolicy")]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PoliticaSeguridadDto>> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            var updatedPolicy = await _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
            return Ok(updatedPolicy);
        }
    }
}
