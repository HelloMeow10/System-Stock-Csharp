using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SharedKernel;
using System;
using System.Collections.Generic;
using Contracts;

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
                { typeof(PersonaDto), resource => ResourceLinker.GetPersonaLinks(((PersonaDto)resource).IdPersona) },
                { typeof(PoliticaSeguridadDto), resource => ResourceLinker.GetSecurityPolicyLinks() }
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

    }
}
