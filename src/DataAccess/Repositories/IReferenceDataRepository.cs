using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IReferenceDataRepository
    {
        List<TipoDoc> GetAllTiposDoc();
        List<Genero> GetAllGeneros();
        List<Rol> GetAllRoles();
        List<Provincia> GetAllProvincias();
        List<Partido> GetPartidosByProvinciaId(int provinciaId);
        List<Localidad> GetLocalidadesByPartidoId(int partidoId);
        TipoDoc? GetTipoDocByNombre(string nombre);
        Localidad? GetLocalidadByNombre(string nombre);
        Genero? GetGeneroByNombre(string nombre);
        Rol? GetRolByNombre(string nombre);
    }
}
