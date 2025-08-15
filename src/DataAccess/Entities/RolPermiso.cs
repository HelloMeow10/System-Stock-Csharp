using System.ComponentModel.DataAnnotations.Schema;

// src/DataAccess/Entities/RolPermiso.cs
namespace DataAccess.Entities
{
    public class RolPermiso
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
        public Rol? Rol { get; set; }
        public Permiso? Permiso { get; set; }
    }
}