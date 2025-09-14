using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Contracts;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    public class UserLinksFilter : IActionFilter
    {
        private readonly ILinkService _linkService;

        public UserLinksFilter(ILinkService linkService)
        {
            _linkService = linkService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No logic needed before the action executes.
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult okResult)
            {
                if (okResult.Value is ApiResponse<UserDto> apiResponse && apiResponse.Data != null)
                {
                    _linkService.AddLinks(apiResponse.Data);
                }
                else if (okResult.Value is PagedApiResponse<IEnumerable<UserDto>> pagedApiResponse)
                {
                    pagedApiResponse.Data.ToList().ForEach(user => _linkService.AddLinks(user));
                }
                else if (okResult.Value is ApiResponse<PersonaDto> personaResponse && personaResponse.Data != null)
                {
                    _linkService.AddLinks(personaResponse.Data);
                }
                else if (okResult.Value is PagedApiResponse<IEnumerable<PersonaDto>> pagedPersonaResponse)
                {
                    pagedPersonaResponse.Data.ToList().ForEach(persona => _linkService.AddLinks(persona));
                }
                else if (okResult.Value is ApiResponse<PoliticaSeguridadDto> politicaResponse && politicaResponse.Data != null)
                {
                    _linkService.AddLinks(politicaResponse.Data);
                }
            }
        }
    }
}
