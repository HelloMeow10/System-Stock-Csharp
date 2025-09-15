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

            var selfLink = urlHelper.Link("GetUserById", new { id = userId });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdateUser", new { id = userId });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-user", "PUT"));
            }

            var patchLink = urlHelper.Link("PatchUser", new { id = userId });
            if (patchLink != null)
            {
                resource.Links.Add(new LinkDto(patchLink, "partially-update-user", "PATCH"));
            }

            var deleteLink = urlHelper.Link("DeleteUser", new { id = userId });
            if (deleteLink != null)
            {
                resource.Links.Add(new LinkDto(deleteLink, "delete-user", "DELETE"));
            }
        }
    }
}
