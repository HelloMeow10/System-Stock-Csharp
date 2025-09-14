using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    /// <summary>
    /// Represents the data required to update an existing persona.
    /// </summary>
    public class UpdatePersonaRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = null!;

        [Required]
        public int IdTipoDoc { get; set; }

        [Required]
        [StringLength(20)]
        public string NumDoc { get; set; } = null!;

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(11)]
        public string? Cuil { get; set; }

        [StringLength(100)]
        public string? Calle { get; set; }

        [StringLength(10)]
        public string? Altura { get; set; }

        [Required]
        public int IdLocalidad { get; set; }

        [Required]
        public int IdPartido { get; set; }

        [Required]
        public int IdProvincia { get; set; }

        [Required]
        public int IdGenero { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }

        [StringLength(20)]
        public string? Celular { get; set; }
    }
}
