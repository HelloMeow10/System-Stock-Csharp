using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using SharedKernel;

namespace BusinessLogic.Mappers
{
    public static class PagedListMapper
    {
        public static PagedResponse<TDto> ToPagedResponse<T, TDto>(this PagedList<T> pagedList, Func<T, TDto> map) where TDto : class
        {
            var dtos = pagedList.Items.Select(map).ToList();
            return new PagedResponse<TDto>(dtos, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
        }
    }
}
