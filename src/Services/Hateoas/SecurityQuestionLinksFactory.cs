using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class SecurityQuestionLinksFactory : ILinkFactory<PreguntaSeguridadDto>
    {
        public void AddLinks(PreguntaSeguridadDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetSecurityQuestionById", new { id = resource.IdPregunta }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
