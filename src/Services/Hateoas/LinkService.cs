using System.Collections.Generic;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public class LinkService : ILinkService
    {
        public void AddLinksForUser(IUrlHelper urlHelper, UserDto user)
        {
            user.Links.Add(new LinkDto(urlHelper.Link("GetUserById", new { id = user.IdUsuario })!, "self", "GET"));
            user.Links.Add(new LinkDto(urlHelper.Link("DeleteUser", new { id = user.IdUsuario })!, "delete_user", "DELETE"));
            user.Links.Add(new LinkDto(urlHelper.Link("UpdateUser", new { id = user.IdUsuario })!, "update_user", "PUT"));
        }

        public void AddLinksForPersona(IUrlHelper urlHelper, PersonaDto persona)
        {
            persona.Links.Add(new LinkDto(urlHelper.Link("GetPersonaById", new { id = persona.IdPersona })!, "self", "GET"));
            persona.Links.Add(new LinkDto(urlHelper.Link("DeletePersona", new { id = persona.IdPersona })!, "delete_persona", "DELETE"));
            persona.Links.Add(new LinkDto(urlHelper.Link("UpdatePersona", new { id = persona.IdPersona })!, "update_persona", "PUT"));
        }

        public List<LinkDto> CreateLinksForCollection<T>(IUrlHelper urlHelper, PagedList<T> pagedList, string routeName, PaginationParams paginationParams)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreateResourceUri(urlHelper, routeName, paginationParams.PageNumber, paginationParams.PageSize), "self", "GET"));

            if (pagedList.HasNext)
            {
                links.Add(new LinkDto(CreateResourceUri(urlHelper, routeName, pagedList.CurrentPage + 1, pagedList.PageSize), "nextPage", "GET"));
            }

            if (pagedList.HasPrevious)
            {
                links.Add(new LinkDto(CreateResourceUri(urlHelper, routeName, pagedList.CurrentPage - 1, pagedList.PageSize), "previousPage", "GET"));
            }

            return links;
        }

        private string CreateResourceUri(IUrlHelper urlHelper, string routeName, int pageNumber, int pageSize)
        {
            return urlHelper.Link(routeName, new
            {
                pageNumber = pageNumber,
                pageSize = pageSize
            })!;
        }
    }
}
