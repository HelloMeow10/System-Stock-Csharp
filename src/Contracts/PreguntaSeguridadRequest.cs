using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class PreguntaSeguridadRequest
    {
        [Required]
        [MaxLength(255)]
        public string Pregunta { get; set; } = string.Empty;
    }
}
