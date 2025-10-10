using SharedKernel;

namespace Contracts
{
    public class PreguntaSeguridadDto : ResourceDto
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; } = null!;
    }
}
