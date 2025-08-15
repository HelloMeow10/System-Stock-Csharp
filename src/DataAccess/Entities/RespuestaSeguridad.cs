using System.ComponentModel.DataAnnotations.Schema;

// src/DataAccess/Entities/RespuestaSeguridad.cs
namespace DataAccess.Entities
{
    public class RespuestaSeguridad
    {
        public int IdRespuesta { get; set; }
        public int IdUsuario { get; set; }
        public int IdPregunta { get; set; }
        public string Respuesta { get; set; } = null!;
        public Usuario? Usuario { get; set; }
        public PreguntaSeguridad? Pregunta { get; set; }
    }
}