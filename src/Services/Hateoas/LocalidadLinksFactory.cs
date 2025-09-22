using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class LocalidadLinksFactory : ILinkFactory<LocalidadDto>
    {
        public void AddLinks(LocalidadDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetLocalidadById", new { id = resource.IdLocalidad }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
