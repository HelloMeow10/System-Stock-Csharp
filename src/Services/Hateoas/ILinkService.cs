using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    /// <summary>
    /// Represents the specification for creating a HATEOAS link.
    /// </summary>
    public class LinkSpec
    {
        public string RouteName { get; }
        public object? RouteValues { get; }
        public string Rel { get; }
        public string Method { get; }

        public LinkSpec(string routeName, object? routeValues, string rel, string method)
        {
            RouteName = routeName;
            RouteValues = routeValues;
            Rel = rel;
            Method = method;
        }
    }

    /// <summary>
    /// A service for creating HATEOAS links.
    /// </summary>
    public interface ILinkService
    {
        /// <summary>
        /// Adds a set of HATEOAS links to a resource.
        /// </summary>
        /// <typeparam name="T">The type of the resource, must inherit from ResourceDto.</typeparam>
        /// <param name="resource">The resource to add links to.</param>
        /// <param name="linkSpecs">The specifications for the links to add.</param>
        void AddLinksToResource<T>(T resource, IEnumerable<LinkSpec> linkSpecs) where T : ResourceDto;

        /// <summary>
        /// Creates a list of HATEOAS links for a paginated collection of resources.
        /// </summary>
        /// <typeparam name="T">The type of the items in the paged list.</typeparam>
        /// <param name="pagedList">The paginated list.</param>
        /// <param name="routeName">The name of the route for the collection.</param>
        /// <param name="paginationParams">The pagination parameters used to retrieve the list.</param>
        /// <returns>A list of LinkDto objects for pagination (self, nextPage, previousPage).</returns>
        List<LinkDto> GetLinksForCollection<T>(PagedList<T> pagedList, string routeName, PaginationParams paginationParams);
    }
}
