using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;

namespace Services.Hateoas
{
    public class PagedResponseLinksFactory<T> : ILinkFactory<PagedResponse<T>>
    {
        public PagedResponseLinksFactory()
        {
        }

        public void AddLinks(PagedResponse<T> resource, IUrlHelper urlHelper)
        {
            if (!urlHelper.ActionContext.RouteData.Values.TryGetValue("action", out var routeNameObj) || routeNameObj == null)
            {
                return;
            }
            var routeName = routeNameObj.ToString();

            var queryParams = urlHelper.ActionContext.HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

            var apiVersion = urlHelper.ActionContext.HttpContext.GetRequestedApiVersion();
            if (apiVersion != null)
            {
                // The key must match the route parameter name in the template
                queryParams["version"] = apiVersion.ToString();
            }

            if (resource.PageNumber < resource.TotalPages)
            {
                queryParams["pageNumber"] = (resource.PageNumber + 1).ToString();
                var nextPageUrl = urlHelper.Link(routeName, queryParams);
                if (nextPageUrl != null)
                {
                    resource.Links.Add(new LinkDto(nextPageUrl, "next", "GET"));
                }
            }

            if (resource.PageNumber > 1)
            {
                queryParams["pageNumber"] = (resource.PageNumber - 1).ToString();
                var prevPageUrl = urlHelper.Link(routeName, queryParams);
                if (prevPageUrl != null)
                {
                    resource.Links.Add(new LinkDto(prevPageUrl, "prev", "GET"));
                }
            }

            queryParams["pageNumber"] = "1";
            var firstPageUrl = urlHelper.Link(routeName, queryParams);
            if (firstPageUrl != null)
            {
                resource.Links.Add(new LinkDto(firstPageUrl, "first", "GET"));
            }

            queryParams["pageNumber"] = resource.TotalPages.ToString();
            var lastPageUrl = urlHelper.Link(routeName, queryParams);
            if (lastPageUrl != null)
            {
                resource.Links.Add(new LinkDto(lastPageUrl, "last", "GET"));
            }
        }
    }
}
