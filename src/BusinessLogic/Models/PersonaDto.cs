using System;
using SharedKernel;

namespace BusinessLogic.Models
{
    public class PersonaDto : ResourceDto
    {
        public int IdPersona { get; set; }
        public int Legajo { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public int IdTipoDoc { get; set; }
        public string? TipoDocNombre { get; set; }
        public string NumDoc { get; set; } = null!;
        public DateTime? FechaNacimiento { get; set; }
        public string? Cuil { get; set; }
        public string? Calle { get; set; }
        public string? Altura { get; set; }
        public int IdLocalidad { get; set; }
        public string? LocalidadNombre { get; set; }
        public int IdPartido { get; set; }
        public string? PartidoNombre { get; set; }
        public int IdProvincia { get; set; }
        public string? ProvinciaNombre { get; set; }
        public int IdGenero { get; set; }
        public string? GeneroNombre { get; set; }
        public string? Correo { get; set; }
        public string? Celular { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}