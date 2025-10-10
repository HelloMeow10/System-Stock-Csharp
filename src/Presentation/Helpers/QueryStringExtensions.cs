using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using SharedKernel;

namespace Presentation.Helpers
{
    public static class QueryStringExtensions
    {
        public static string ToQueryString(this UserQueryParameters query)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(query.Username))
                parts.Add($"Username={Uri.EscapeDataString(query.Username)}");
            if (!string.IsNullOrWhiteSpace(query.Email))
                parts.Add($"Email={Uri.EscapeDataString(query.Email)}");
            if (!string.IsNullOrWhiteSpace(query.SortBy))
                parts.Add($"SortBy={Uri.EscapeDataString(query.SortBy)}");
            parts.Add($"PageNumber={query.PageNumber}");
            parts.Add($"PageSize={query.PageSize}");
            return string.Join("&", parts);
        }

        public static string ToQueryString(this PaginationParams pagination)
        {
            return $"PageNumber={pagination.PageNumber}&PageSize={pagination.PageSize}";
        }
    }
}
