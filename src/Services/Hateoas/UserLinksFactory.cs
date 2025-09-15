using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class UserLinksFactory : ILinkFactory<UserDto>
    {
        public void AddLinks(UserDto resource, IUrlHelper urlHelper)
        {
            var userId = resource.IdUsuario;

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetUserById", new { id = userId }),
                rel: "self",
                method: "GET"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("UpdateUser", new { id = userId }),
                rel: "update-user",
                method: "PUT"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("PatchUser", new { id = userId }),
                rel: "partially-update-user",
                method: "PATCH"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("DeleteUser", new { id = userId }),
                rel: "delete-user",
                method: "DELETE"));
        }
    }
}
