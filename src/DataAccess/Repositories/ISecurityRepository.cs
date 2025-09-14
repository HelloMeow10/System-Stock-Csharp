using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ISecurityRepository
    {
        Task<PoliticaSeguridad?> GetPoliticaSeguridadAsync();
        Task UpdatePoliticaSeguridadAsync(PoliticaSeguridad politica);
        List<PreguntaSeguridad> GetPreguntasSeguridad();
        List<PreguntaSeguridad> GetPreguntasSeguridadByIds(List<int> ids);
        List<RespuestaSeguridad>? GetRespuestasSeguridadByUsuarioId(int idUsuario);
        void AddRespuestaSeguridad(RespuestaSeguridad respuesta);
        void DeleteRespuestasSeguridadByUsuarioId(int usuarioId);
    }
}
