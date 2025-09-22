using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class PartidoLinksFactory : ILinkFactory<PartidoDto>
    {
        public void AddLinks(PartidoDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetPartidoById", new { id = resource.IdPartido }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
