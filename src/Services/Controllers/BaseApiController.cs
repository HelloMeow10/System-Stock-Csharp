using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
