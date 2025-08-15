using System.ComponentModel.DataAnnotations;

// src/DataAccess/Entities/PreguntaSeguridad.cs
namespace DataAccess.Entities
{
    public class PreguntaSeguridad
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; } = null!;
    }
}