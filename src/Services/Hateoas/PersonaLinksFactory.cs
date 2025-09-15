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

            var selfLink = urlHelper.Link("GetPersonaById", new { id = personaId });
            if (selfLink != null)
            {
                resource.Links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            var updateLink = urlHelper.Link("UpdatePersona", new { id = personaId });
            if (updateLink != null)
            {
                resource.Links.Add(new LinkDto(updateLink, "update-persona", "PUT"));
            }

            var patchLink = urlHelper.Link("PatchPersona", new { id = personaId });
            if (patchLink != null)
            {
                resource.Links.Add(new LinkDto(patchLink, "partially-update-persona", "PATCH"));
            }

            var deleteLink = urlHelper.Link("DeletePersona", new { id = personaId });
            if (deleteLink != null)
            {
                resource.Links.Add(new LinkDto(deleteLink, "delete-persona", "DELETE"));
            }
        }
    }
}
