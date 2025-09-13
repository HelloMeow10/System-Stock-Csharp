using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    public interface ILinkService
    {
        void AddLinksForUser(IUrlHelper urlHelper, UserDto user);
        void AddLinksForPersona(IUrlHelper urlHelper, PersonaDto persona);
        List<LinkDto> CreateLinksForCollection<T>(IUrlHelper urlHelper, PagedList<T> pagedList, string routeName, PaginationParams paginationParams);
    }
}
