using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ISecurityRepository
    {
        Task<PoliticaSeguridad?> GetPoliticaSeguridadAsync();
        Task UpdatePoliticaSeguridadAsync(PoliticaSeguridad politica);
        Task<List<PreguntaSeguridad>> GetPreguntasSeguridadAsync();
        Task<List<PreguntaSeguridad>> GetPreguntasSeguridadByIdsAsync(List<int> ids);
        Task<List<RespuestaSeguridad>?> GetRespuestasSeguridadByUsuarioIdAsync(int idUsuario);
        Task AddRespuestaSeguridadAsync(RespuestaSeguridad respuesta);
        Task DeleteRespuestasSeguridadByUsuarioIdAsync(int usuarioId);
        Task<int> AddPreguntaSeguridadAsync(string pregunta);
        Task UpdatePreguntaSeguridadAsync(int idPregunta, string pregunta);
        Task DeletePreguntaSeguridadAsync(int idPregunta);
    }
}
