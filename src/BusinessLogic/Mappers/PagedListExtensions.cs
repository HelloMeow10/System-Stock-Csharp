using Contracts;
using SharedKernel;
using System.Collections.Generic;

namespace BusinessLogic.Mappers
{
    public static class PagedListExtensions
    {
        public static PagedResponse<TDto> ToPagedResponse<TEntity, TDto>(this PagedList<TEntity> pagedList, IEnumerable<TDto> items)
        {
            return new PagedResponse<TDto>(
                items,
                pagedList.CurrentPage,
                pagedList.PageSize,
                pagedList.TotalCount
            );
        }
    }
}