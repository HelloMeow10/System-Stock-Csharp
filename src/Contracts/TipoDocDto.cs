using SharedKernel;

namespace Contracts
{
    public class TipoDocDto : ResourceDto
    {
        public int IdTipoDoc { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
