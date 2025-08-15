using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface ISecurityQuestionService
    {
        Task GuardarRespuestasSeguridadAsync(string username, Dictionary<int, string> respuestas);
        Task<List<PreguntaSeguridadDto>> GetPreguntasDeUsuarioAsync(string username);
        List<PreguntaSeguridadDto> GetPreguntasSeguridad(); // This can remain sync as it likely reads from a cached/static list
        PoliticaSeguridadDto? GetPoliticaSeguridad(); // This can also remain sync
    }
}
