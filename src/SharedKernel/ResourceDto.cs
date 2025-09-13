using System.Collections.Generic;

namespace SharedKernel
{
    public abstract class ResourceDto
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
