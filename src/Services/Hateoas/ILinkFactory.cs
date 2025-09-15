using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Services.Hateoas
{
    public interface ILinkFactory<T> where T : ResourceDto
    {
        void AddLinks(T resource, IUrlHelper urlHelper);
    }
}
