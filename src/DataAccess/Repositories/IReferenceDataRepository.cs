using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IReferenceDataRepository
    {
        Task<List<TipoDoc>> GetAllTiposDocAsync();
        Task<List<Genero>> GetAllGenerosAsync();
        Task<List<Rol>> GetAllRolesAsync();
        Task<List<Provincia>> GetAllProvinciasAsync();
        Task<List<Partido>> GetPartidosByProvinciaIdAsync(int provinciaId);
        Task<List<Localidad>> GetLocalidadesByPartidoIdAsync(int partidoId);
        Task<TipoDoc?> GetTipoDocByNombreAsync(string nombre);
        Task<Localidad?> GetLocalidadByNombreAsync(string nombre);
        Task<Genero?> GetGeneroByNombreAsync(string nombre);
        Task<Rol?> GetRolByNombreAsync(string nombre);
    }
}
