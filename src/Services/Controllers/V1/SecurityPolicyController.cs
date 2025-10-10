using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers.V1
{
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    public class SecurityPolicyController : BaseApiController
    {
        private readonly ISecurityPolicyService _securityPolicyService;

        public SecurityPolicyController(ISecurityPolicyService securityPolicyService)
        {
            _securityPolicyService = securityPolicyService;
        }

        [HttpGet(Name = "GetSecurityPolicyV1")]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        public Task<PoliticaSeguridadDto> Get()
        {
            return _securityPolicyService.GetPoliticaSeguridadAsync();
        }

        [HttpPut(Name = "UpdateSecurityPolicyV1")]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        public Task<PoliticaSeguridadDto> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            return _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
        }
    }
}
