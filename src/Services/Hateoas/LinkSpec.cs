namespace Services.Hateoas
{
    public class LinkSpec
    {
        public string RouteName { get; }
        public object RouteValues { get; }
        public string Rel { get; }
        public string Method { get; }

        public LinkSpec(string routeName, object routeValues, string rel, string method)
        {
            RouteName = routeName;
            RouteValues = routeValues;
            Rel = rel;
            Method = method;
        }
    }
}
