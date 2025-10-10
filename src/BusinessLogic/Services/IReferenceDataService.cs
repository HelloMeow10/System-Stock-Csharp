using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;

namespace BusinessLogic.Services
{
    public interface IReferenceDataService
    {
        Task<List<TipoDocDto>> GetTiposDocAsync();
        Task<List<ProvinciaDto>> GetProvinciasAsync();
        Task<List<PartidoDto>> GetPartidosByProvinciaIdAsync(int provinciaId);
        Task<List<LocalidadDto>> GetLocalidadesByPartidoIdAsync(int partidoId);
        Task<List<GeneroDto>> GetGenerosAsync();
        Task<List<RolDto>> GetRolesAsync();
    }
}
