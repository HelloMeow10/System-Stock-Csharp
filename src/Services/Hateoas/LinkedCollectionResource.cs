using System.Collections.Generic;
using SharedKernel;

namespace Services.Hateoas
{
    public class LinkedCollectionResource<T> : ResourceDto where T : ResourceDto
    {
        public List<T> Value { get; set; }

        public LinkedCollectionResource(List<T> value)
        {
            Value = value;
        }
    }
}
