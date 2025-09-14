using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    /// <summary>
    /// Represents the data required to update the security policy.
    /// </summary>
    public class UpdatePoliticaSeguridadRequest
    {
        [Required]
        public bool MayusYMinus { get; set; }

        [Required]
        public bool LetrasYNumeros { get; set; }

        [Required]
        public bool CaracterEspecial { get; set; }

        [Required]
        public bool Autenticacion2FA { get; set; }

        [Required]
        public bool NoRepetirAnteriores { get; set; }

        [Required]
        public bool SinDatosPersonales { get; set; }

        [Required]
        [Range(1, 100)]
        public int MinCaracteres { get; set; }

        [Required]
        [Range(1, 10)]
        public int CantPreguntas { get; set; }
    }
}
