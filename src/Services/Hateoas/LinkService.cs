using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    public class LinkService : ILinkService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public LinkService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        public void AddLinksToResource<T>(T resource, IEnumerable<LinkSpec> linkSpecs) where T : ResourceDto
        {
            if (_actionContextAccessor.ActionContext == null) return;

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            foreach (var spec in linkSpecs)
            {
                var url = urlHelper.Link(spec.RouteName, spec.RouteValues);
                if (url != null)
                {
                    resource.Links.Add(new LinkDto(url, spec.Rel, spec.Method));
                }
            }
        }

        public List<LinkDto> GetLinksForCollection<T>(PagedList<T> pagedList, string routeName, PaginationParams paginationParams)
        {
            var links = new List<LinkDto>();

            if (_actionContextAccessor.ActionContext == null) return links;

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            // Self link for the current page
            var selfLink = urlHelper.Link(routeName, new { pageNumber = paginationParams.PageNumber, pageSize = paginationParams.PageSize });
            if (selfLink != null)
            {
                links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            // Next page link
            if (pagedList.HasNext)
            {
                var nextPageLink = urlHelper.Link(routeName, new { pageNumber = pagedList.CurrentPage + 1, pageSize = pagedList.PageSize });
                if (nextPageLink != null)
                {
                    links.Add(new LinkDto(nextPageLink, "nextPage", "GET"));
                }
            }

            // Previous page link
            if (pagedList.HasPrevious)
            {
                var prevPageLink = urlHelper.Link(routeName, new { pageNumber = pagedList.CurrentPage - 1, pageSize = pagedList.PageSize });
                if (prevPageLink != null)
                {
                    links.Add(new LinkDto(prevPageLink, "previousPage", "GET"));
                }
            }

            return links;
        }
    }
}
