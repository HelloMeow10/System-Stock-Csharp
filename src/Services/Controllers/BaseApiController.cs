using Microsoft.AspNetCore.Mvc;
using Services.Hateoas;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(UserLinksFilter))]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
