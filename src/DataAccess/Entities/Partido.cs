using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Partido
    {
        public int IdPartido { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdProvincia { get; set; }
        public Provincia Provincia { get; set; } = null!;
    }
}