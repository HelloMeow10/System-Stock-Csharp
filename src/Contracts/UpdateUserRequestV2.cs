using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    /// <summary>
    /// Represents the data required to update an existing user for API v2.
    /// </summary>
    public class UpdateUserRequestV2
    {
        [Required]
        [StringLength(201)] // Accommodate combined first and last names
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Correo { get; set; }

        [Required]
        public int IdRol { get; set; }

        [Required]
        public bool CambioContrasenaObligatorio { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        [Required]
        public bool Habilitado { get; set; }
    }
}
