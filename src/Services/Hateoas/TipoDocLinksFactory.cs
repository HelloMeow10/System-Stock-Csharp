using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class TipoDocLinksFactory : ILinkFactory<TipoDocDto>
    {
        public void AddLinks(TipoDocDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetTipoDocById", new { id = resource.IdTipoDoc }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
