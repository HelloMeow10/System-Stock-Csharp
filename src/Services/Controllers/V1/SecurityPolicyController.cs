using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Hateoas;
using BusinessLogic.Exceptions;
using Services.Authentication;
using Asp.Versioning;

namespace Services.Controllers.V1
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
        public Task<PoliticaSeguridadDto> Get()
        {
            return _securityPolicyService.GetPoliticaSeguridadAsync();
        }

        [HttpPut(Name = "UpdateSecurityPolicy")]
        [ProducesResponseType(typeof(PoliticaSeguridadDto), StatusCodes.Status200OK)]
        public Task<PoliticaSeguridadDto> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            return _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
        }
    }
}
