using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface ISecurityQuestionService
    {
        Task SaveSecurityAnswersAsync(string username, Dictionary<int, string> answers);
        Task<List<PreguntaSeguridadDto>> GetUserSecurityQuestionsAsync(string username);
        List<PreguntaSeguridadDto> GetSecurityQuestions(); // This can remain sync as it likely reads from a cached/static list
        Task<PoliticaSeguridadDto?> GetSecurityPolicyAsync();
    }
}
