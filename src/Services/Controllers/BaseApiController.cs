using Microsoft.AspNetCore.Mvc;
using Services.Hateoas;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ServiceFilter(typeof(HateoasActionFilter))]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
