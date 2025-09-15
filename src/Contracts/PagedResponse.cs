using System.Collections.Generic;
using SharedKernel;

namespace Contracts
{
    public class PagedResponse<T> : ResourceDto
    {
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)System.Math.Ceiling(totalRecords / (double)pageSize);
            TotalRecords = totalRecords;
        }
    }
}
