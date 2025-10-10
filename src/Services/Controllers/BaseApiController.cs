using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Hateoas;
using System.Linq;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ServiceFilter(typeof(HateoasActionFilter))]
    public abstract class BaseApiController : ControllerBase
    {
        protected void ApplyPatchAndValidate<T>(JsonPatchDocument<T> patchDoc, T modelToApplyTo) where T : class
        {
            patchDoc.ApplyTo(modelToApplyTo, ModelState);
            TryValidateModel(modelToApplyTo);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                throw new ValidationException(errors);
            }
        }
    }
}
