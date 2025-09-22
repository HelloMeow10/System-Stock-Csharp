using SharedKernel;

namespace Contracts
{
    public class ProvinciaDto : ResourceDto
    {
        public int IdProvincia { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
