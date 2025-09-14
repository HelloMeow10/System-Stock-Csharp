using System.Collections.Generic;
using SharedKernel;

namespace Contracts
{
    public class PagedApiResponse<T> : ApiResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

        public PagedApiResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)System.Math.Ceiling(totalRecords / (double)pageSize);
            TotalRecords = totalRecords;
            Data = data;
            Succeeded = true;
        }
    }
}
