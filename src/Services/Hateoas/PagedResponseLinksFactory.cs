using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace Services.Hateoas
{
    public class PagedResponseLinksFactory<T> : ILinkFactory<PagedResponse<IEnumerable<T>>> where T : ResourceDto
    {
        public PagedResponseLinksFactory()
        {
        }

        public void AddLinks(PagedResponse<IEnumerable<T>> resource, IUrlHelper urlHelper)
        {
            var routeName = urlHelper.ActionContext.RouteData.Values["action"].ToString();
            var queryParams = urlHelper.ActionContext.HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

            if (resource.PageNumber < resource.TotalPages)
            {
                queryParams["pageNumber"] = (resource.PageNumber + 1).ToString();
                var nextPageUrl = urlHelper.Link(routeName, queryParams);
                resource.Links.Add(new LinkDto(nextPageUrl, "next", "GET"));
            }

            if (resource.PageNumber > 1)
            {
                queryParams["pageNumber"] = (resource.PageNumber - 1).ToString();
                var prevPageUrl = urlHelper.Link(routeName, queryParams);
                resource.Links.Add(new LinkDto(prevPageUrl, "prev", "GET"));
            }

            queryParams["pageNumber"] = "1";
            var firstPageUrl = urlHelper.Link(routeName, queryParams);
            resource.Links.Add(new LinkDto(firstPageUrl, "first", "GET"));

            queryParams["pageNumber"] = resource.TotalPages.ToString();
            var lastPageUrl = urlHelper.Link(routeName, queryParams);
            resource.Links.Add(new LinkDto(lastPageUrl, "last", "GET"));
        }
    }
}
