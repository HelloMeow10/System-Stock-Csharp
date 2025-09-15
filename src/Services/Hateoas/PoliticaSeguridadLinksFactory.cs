using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class PoliticaSeguridadLinksFactory : ILinkFactory<PoliticaSeguridadDto>
    {
        public void AddLinks(PoliticaSeguridadDto resource, IUrlHelper urlHelper)
        {
            var selfLink = urlHelper.Link("GetSecurityPolicy", null);
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdateSecurityPolicy", null);
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-policy", "PUT"));
            }
        }
    }
}
