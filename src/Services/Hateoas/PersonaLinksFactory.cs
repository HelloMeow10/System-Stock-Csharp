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

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("GetPersonaById", new { id = personaId }),
                rel: "self",
                method: "GET"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("UpdatePersona", new { id = personaId }),
                rel: "update-persona",
                method: "PUT"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("PatchPersona", new { id = personaId }),
                rel: "partially-update-persona",
                method: "PATCH"));

            resource.Links.Add(new LinkDto(
                href: urlHelper.Link("DeletePersona", new { id = personaId }),
                rel: "delete-persona",
                method: "DELETE"));
        }
    }
}
