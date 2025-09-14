using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SharedKernel;
using System;
using System.Collections.Generic;
using BusinessLogic.Models;

namespace Services.Hateoas
{
    public class LinkService : ILinkService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly Dictionary<Type, Func<ResourceDto, List<LinkSpec>>> _linkFactories;

        public LinkService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;

            _linkFactories = new Dictionary<Type, Func<ResourceDto, List<LinkSpec>>>
            {
                { typeof(UserDto), resource => ResourceLinker.GetUserLinks(((UserDto)resource).IdUsuario) },
                { typeof(PersonaDto), resource => ResourceLinker.GetPersonaLinks(((PersonaDto)resource).IdPersona) }
            };
        }

        public void AddLinks(ResourceDto resource)
        {
            if (resource == null) return;

            if (_linkFactories.TryGetValue(resource.GetType(), out var factory))
            {
                var linkSpecs = factory(resource);
                AddLinksToResource(resource, linkSpecs);
            }
        }

        private void AddLinksToResource(ResourceDto resource, IEnumerable<LinkSpec> linkSpecs)
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

            var selfLink = urlHelper.Link(routeName, new { pageNumber = paginationParams.PageNumber, pageSize = paginationParams.PageSize });
            if (selfLink != null)
            {
                links.Add(new LinkDto(selfLink, "self", "GET"));
            }

            if (pagedList.HasNext)
            {
                var nextPageLink = urlHelper.Link(routeName, new { pageNumber = pagedList.CurrentPage + 1, pageSize = pagedList.PageSize });
                if (nextPageLink != null)
                {
                    links.Add(new LinkDto(nextPageLink, "nextPage", "GET"));
                }
            }

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
