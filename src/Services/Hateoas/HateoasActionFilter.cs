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
using Microsoft.AspNetCore.Http;

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
            if (context.Result is not ObjectResult objectResult || objectResult.Value == null)
            {
                return;
            }

            int? statusCode = objectResult.StatusCode;

            if (!statusCode.HasValue)
            {
                switch (context.Result)
                {
                    case CreatedAtActionResult _:
                        statusCode = StatusCodes.Status201Created;
                        break;
                    case OkObjectResult _:
                        statusCode = StatusCodes.Status200OK;
                        break;
                }
            }

            if (statusCode.HasValue && statusCode.Value >= 200 && statusCode.Value < 300)
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
                if (pagedResponse.Items is IEnumerable data)
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
            var pagedResponseType = pagedResponse.GetType(); // e.g., PagedResponse<UserDto>
            var itemType = pagedResponseType.GetGenericArguments().First(); // e.g., UserDto

            // We need to construct the concrete factory type PagedResponseLinksFactory<UserDto>
            var factoryGenericType = typeof(PagedResponseLinksFactory<>);
            var factorySpecificType = factoryGenericType.MakeGenericType(itemType);

            // And resolve that concrete type from the container
            var factory = _serviceProvider.GetService(factorySpecificType);

            if (factory != null)
            {
                var addLinksMethod = factory.GetType().GetMethod("AddLinks");
                addLinksMethod?.Invoke(factory, new object[] { pagedResponse, urlHelper });
            }
        }
    }
}
