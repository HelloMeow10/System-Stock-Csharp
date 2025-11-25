using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;

namespace BusinessLogic.Services
{
    public interface ISecurityQuestionService
    {
        Task SaveSecurityAnswersAsync(string username, Dictionary<int, string> answers);
        Task<List<PreguntaSeguridadDto>> GetUserSecurityQuestionsAsync(string username);
        Task<List<PreguntaSeguridadDto>> GetSecurityQuestionsAsync();
        Task<PreguntaSeguridadDto> CreateSecurityQuestionAsync(string pregunta);
        Task UpdateSecurityQuestionAsync(int idPregunta, string pregunta);
        Task DeleteSecurityQuestionAsync(int idPregunta);
    }
}
