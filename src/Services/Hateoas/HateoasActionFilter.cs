using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SharedKernel;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Contracts;

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
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value != null &&
                objectResult.StatusCode.HasValue &&
                objectResult.StatusCode.Value >= 200 &&
                objectResult.StatusCode.Value < 300)
            {
                var urlHelper = _urlHelperFactory.GetUrlHelper(context);
                ProcessHateoas(objectResult.Value, urlHelper);
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
                var pagedResponse = (dynamic)value;
                if (pagedResponse.Data is IEnumerable data)
                {
                    foreach (var item in data)
                    {
                        if (item is ResourceDto itemResource)
                        {
                            AddLinksToResource(itemResource, urlHelper);
                        }
                    }
                }
                AddLinksToPagedResponse(pagedResponse, urlHelper);
            }
            else if (value is IEnumerable enumerable && !(value is string))
            {
                foreach (var item in enumerable)
                {
                    if (item is ResourceDto itemResource)
                    {
                        AddLinksToResource(itemResource, urlHelper);
                    }
                }
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

        private void AddLinksToPagedResponse(object pagedResponse, IUrlHelper urlHelper)
        {
            var pagedResponseType = pagedResponse.GetType();
            var factoryType = typeof(ILinkFactory<>).MakeGenericType(pagedResponseType);
            var factory = _serviceProvider.GetService(factoryType);

            if (factory != null)
            {
                var addLinksMethod = factory.GetType().GetMethod("AddLinks");
                addLinksMethod?.Invoke(factory, new object[] { pagedResponse, urlHelper });
            }
        }
    }
}
