using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class RolLinksFactory : ILinkFactory<RolDto>
    {
        public void AddLinks(RolDto resource, IUrlHelper urlHelper)
        {
            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetRolById", new { id = resource.IdRol }),
                rel: "self",
                method: "GET"
            ));
        }
    }
}
