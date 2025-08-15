using System.ComponentModel.DataAnnotations;

// src/DataAccess/Entities/Permiso.cs
namespace DataAccess.Entities
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = null!;
    }
}