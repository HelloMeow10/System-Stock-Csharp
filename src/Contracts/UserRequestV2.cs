using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    /// <summary>
    /// Represents the data required to create a new user and their associated persona in a single call for API v2.
    /// </summary>
    public class UserRequestV2
    {
        [Required]
        [StringLength(201)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        public string Rol { get; set; } = null!;

        // Other persona fields can be added here if needed, e.g., Legajo, Cuil, etc.
        // For now, we'll keep it simple.
    }
}
