using System.Collections.Generic;
using Contracts;

namespace BusinessLogic.Services
{
    public interface IReferenceDataService
    {
        List<TipoDocDto> GetTiposDoc();
        List<ProvinciaDto> GetProvincias();
        List<PartidoDto> GetPartidosByProvinciaId(int provinciaId);
        List<LocalidadDto> GetLocalidadesByPartidoId(int partidoId);
        List<GeneroDto> GetGeneros();
        List<RolDto> GetRoles();
    }
}
