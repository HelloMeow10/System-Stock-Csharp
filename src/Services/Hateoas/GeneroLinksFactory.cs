using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class GeneroLinksFactory : ILinkFactory<GeneroDto>
    {
        public void AddLinks(GeneroDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetGeneroById", new { id = resource.IdGenero }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
