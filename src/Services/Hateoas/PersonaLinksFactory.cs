using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class PersonaLinksFactory : ILinkFactory<PersonaDto>
    {
        public void AddLinks(PersonaDto resource, IUrlHelper urlHelper)
        {
            var personaId = resource.IdPersona;
            var version = "1.0";

            var selfLink = urlHelper.Link("GetPersonaByIdV1", new { id = personaId, version });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdatePersonaV1", new { id = personaId, version });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-persona", "PUT"));
            }

            var patchLink = urlHelper.Link("PatchPersonaV1", new { id = personaId, version });
            if (patchLink != null)
            {
                resource.Links.Add(new LinkDto(patchLink, "partially-update-persona", "PATCH"));
            }

            var deleteLink = urlHelper.Link("DeletePersonaV1", new { id = personaId, version });
            if (deleteLink != null)
            {
                resource.Links.Add(new LinkDto(deleteLink, "delete-persona", "DELETE"));
            }
        }
    }
}
