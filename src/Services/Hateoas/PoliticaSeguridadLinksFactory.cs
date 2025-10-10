using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class PoliticaSeguridadLinksFactory : ILinkFactory<PoliticaSeguridadDto>
    {
        public void AddLinks(PoliticaSeguridadDto resource, IUrlHelper urlHelper)
        {
            var version = "1.0";

            var selfLink = urlHelper.Link("GetSecurityPolicyV1", new { version });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdateSecurityPolicyV1", new { version });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-policy", "PUT"));
            }
        }
    }
}
