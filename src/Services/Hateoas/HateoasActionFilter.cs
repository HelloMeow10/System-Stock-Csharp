using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SharedKernel;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Contracts; // For PagedResponse

namespace Services.Hateoas
{
    public class HateoasActionFilter : IActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public HateoasActionFilter(IServiceProvider serviceProvider, IUrlHelperFactory urlHelperFactory)
        {
            _serviceProvider = serviceProvider;
            _urlHelperFactory = urlHelperFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult okResult && okResult.Value != null)
            {
                var urlHelper = _urlHelperFactory.GetUrlHelper(context);
                ProcessHateoas(okResult.Value, urlHelper);
            }
        }

        private void ProcessHateoas(object value, IUrlHelper urlHelper)
        {
            if (value is ResourceDto resource)
            {
                AddLinksToResource(resource, urlHelper);
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(PagedResponse<>))
            {
                // This handles PagedResponse<IEnumerable<ResourceDto>>
                var data = value.GetType().GetProperty("Data")?.GetValue(value) as IEnumerable;
                if (data == null) return;

                foreach (var item in data)
                {
                    if (item is ResourceDto itemResource)
                    {
                        AddLinksToResource(itemResource, urlHelper);
                    }
                }
                // We can also add pagination links to the PagedResponse itself here if needed in a generic way.
            }
        }

        private void AddLinksToResource(ResourceDto resource, IUrlHelper urlHelper)
        {
            var resourceType = resource.GetType();
            var factoryType = typeof(ILinkFactory<>).MakeGenericType(resourceType);
            var factory = _serviceProvider.GetService(factoryType);

            if (factory != null)
            {
                var addLinksMethod = factory.GetType().GetMethod("AddLinks");
                addLinksMethod?.Invoke(factory, new object[] { resource, urlHelper });
            }
        }
    }
}
