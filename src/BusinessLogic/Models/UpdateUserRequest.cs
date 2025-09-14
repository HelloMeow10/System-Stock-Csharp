using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    /// <summary>
    /// Represents the data required to update an existing user.
    /// </summary>
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string? Apellido { get; set; }

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
