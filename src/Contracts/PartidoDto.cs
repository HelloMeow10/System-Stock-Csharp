using SharedKernel;

namespace Contracts
{
    public class PartidoDto : ResourceDto
    {
        public int IdPartido { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdProvincia { get; set; }
    }
}
