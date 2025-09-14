using BusinessLogic.Models;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    public interface ILinkService
    {
        void AddLinks(ResourceDto resource);
        List<LinkDto> GetLinksForCollection<T>(PagedList<T> pagedList, string routeName, PaginationParams paginationParams);
    }
}
