using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class ProvinciaLinksFactory : ILinkFactory<ProvinciaDto>
    {
        public void AddLinks(ProvinciaDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetProvinciaById", new { id = resource.IdProvincia }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
