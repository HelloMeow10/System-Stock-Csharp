using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class PoliticaSeguridadLinksFactory : ILinkFactory<PoliticaSeguridadDto>
    {
        public void AddLinks(PoliticaSeguridadDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetSecurityPolicy", null),
                rel: "self",
                method: "GET"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("UpdateSecurityPolicy", null),
                rel: "update-security-policy",
                method: "PUT"));
        }
    }
}
