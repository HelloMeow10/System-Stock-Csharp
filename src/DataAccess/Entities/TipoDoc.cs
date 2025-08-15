using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class TipoDoc
    {
        public int IdTipoDoc { get; set; }
        public string Nombre { get; set; } = null!;
    }
}