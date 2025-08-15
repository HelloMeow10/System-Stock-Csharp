using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Provincia
    {
        public int IdProvincia { get; set; }
        public string Nombre { get; set; } = null!;
    }
}