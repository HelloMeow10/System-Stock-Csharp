using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected LinkDto CreateLink(string endpointName, object? values, string rel, string method)
        {
            var uri = Url.Link(endpointName, values) ?? string.Empty;
            return new LinkDto(uri, rel, method);
        }
    }
}
