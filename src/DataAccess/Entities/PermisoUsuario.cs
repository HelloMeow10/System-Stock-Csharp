using System.ComponentModel.DataAnnotations.Schema;

// src/DataAccess/Entities/PermisoUsuario.cs
namespace DataAccess.Entities
{
    public class PermisoUsuario
    {
        public int IdUsuario { get; set; }
        public int IdPermiso { get; set; }
        public Usuario? Usuario { get; set; }
        public Permiso? Permiso { get; set; }
    }
}