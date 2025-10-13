using SharedKernel;

namespace Contracts
{
    public class UserQueryParameters : PaginationParams
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? SortBy { get; set; }
        public int? RoleId { get; set; }
    }
}
