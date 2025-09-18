using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class UserLinksFactoryV2 : ILinkFactory<UserDtoV2>
    {
        public void AddLinks(UserDtoV2 resource, IUrlHelper urlHelper)
        {
            var userId = resource.IdUsuario;
            var version = "2.0";

            var selfLink = urlHelper.Link("GetUserByIdV2", new { id = userId, version });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdateUserV2", new { id = userId, version });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-user", "PUT"));
            }

            var patchLink = urlHelper.Link("PatchUserV2", new { id = userId, version });
            if (patchLink != null)
            {
                resource.Links.Add(new LinkDto(patchLink, "partially-update-user", "PATCH"));
            }

            var deleteLink = urlHelper.Link("DeleteUserV2", new { id = userId, version });
            if (deleteLink != null)
            {
                resource.Links.Add(new LinkDto(deleteLink, "delete-user", "DELETE"));
            }
        }
    }
}
