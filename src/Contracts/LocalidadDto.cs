using SharedKernel;

namespace Contracts
{
    public class LocalidadDto : ResourceDto
    {
        public int IdLocalidad { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdPartido { get; set; }
    }
}
