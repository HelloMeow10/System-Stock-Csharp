using Contracts;
using SharedKernel;
using System.Collections.Generic;

namespace Services.Hateoas
{
    public interface ILinkService
    {
        void AddLinks(ResourceDto resource);
    }
}
