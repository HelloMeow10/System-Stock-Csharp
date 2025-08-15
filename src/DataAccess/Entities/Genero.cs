using System.ComponentModel.DataAnnotations;

// src/DataAccess/Entities/Genero.cs
namespace DataAccess.Entities
{
    public class Genero
    {
        public int IdGenero { get; set; }
        public string Nombre { get; set; } = null!;
    }
}