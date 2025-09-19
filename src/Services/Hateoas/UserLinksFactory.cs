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
            var version = "1.0";

            var selfLink = urlHelper.Link("GetUserByIdV1", new { id = userId, version });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdateUserV1", new { id = userId, version });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-user", "PUT"));
            }

            var patchLink = urlHelper.Link("PatchUserV1", new { id = userId, version });
            if (patchLink != null)
            {
                resource.Links.Add(new LinkDto(patchLink, "partially-update-user", "PATCH"));
            }

            var deleteLink = urlHelper.Link("DeleteUser", new { id = userId, version });
            if (deleteLink != null)
            {
                resource.Links.Add(new LinkDto(deleteLink, "delete-user", "DELETE"));
            }
        }
    }
}
