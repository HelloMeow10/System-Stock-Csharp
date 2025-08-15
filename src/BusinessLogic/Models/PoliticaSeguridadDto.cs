namespace BusinessLogic.Models
{
    public class PoliticaSeguridadDto
    {
        public int IdPolitica { get; set; }
        public bool MayusYMinus { get; set; }
        public bool LetrasYNumeros { get; set; }
        public bool CaracterEspecial { get; set; }
        public bool Autenticacion2FA { get; set; }
        public bool NoRepetirAnteriores { get; set; }
        public bool SinDatosPersonales { get; set; }
        public int MinCaracteres { get; set; }
        public int CantPreguntas { get; set; }
    }
}
