using System.Collections.Generic;
using SharedKernel;

namespace Contracts
{
    public class PagedResponse<T> : ResourceDto
    {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalRecords)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)System.Math.Ceiling(totalRecords / (double)pageSize);
            TotalRecords = totalRecords;
        }
    }
}
