using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Hateoas;

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
        [ProducesResponseType(typeof(ApiResponse<PoliticaSeguridadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PoliticaSeguridadDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var politica = await _securityPolicyService.GetPoliticaSeguridadAsync();
            if (politica == null)
            {
                return NotFound(ApiResponse<PoliticaSeguridadDto>.Fail("Security policy not found."));
            }
            _linkService.AddLinks(politica);
            return Ok(ApiResponse<PoliticaSeguridadDto>.Success(politica));
        }

        [HttpPut(Name = "UpdateSecurityPolicy")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdatePoliticaSeguridadRequest request)
        {
            await _securityPolicyService.UpdatePoliticaSeguridadAsync(request);
            return Ok(ApiResponse<object>.Success(new { message = "Security policy updated successfully." }));
        }
    }
}
