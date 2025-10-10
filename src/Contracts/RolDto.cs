using SharedKernel;

namespace Contracts
{
    public class RolDto : ResourceDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
