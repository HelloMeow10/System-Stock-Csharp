using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Localidad
    {
        public int IdLocalidad { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdPartido { get; set; }
        public Partido Partido { get; set; } = null!;
    }
}