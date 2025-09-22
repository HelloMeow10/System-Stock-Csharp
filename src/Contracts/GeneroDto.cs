using SharedKernel;

namespace Contracts
{
    public class GeneroDto : ResourceDto
    {
        public int IdGenero { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
