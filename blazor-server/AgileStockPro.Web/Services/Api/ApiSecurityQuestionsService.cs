using AgileStockPro.Web.Services.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgileStockPro.Web.Services.Api
{
    public interface ISecurityQuestionsAdminService
    {
        Task<IReadOnlyList<PreguntaSeguridadDto>> GetAllAsync();
        Task<PreguntaSeguridadDto?> CreateAsync(string pregunta);
        Task<bool> UpdateAsync(int idPregunta, string pregunta);
        Task<bool> DeleteAsync(int idPregunta);
        Task<bool> SaveAnswersAsync(string username, Dictionary<int, string> answers);
    }

    public class ApiSecurityQuestionsService : ISecurityQuestionsAdminService
    {
        private readonly BackendApiClient _api;
        public ApiSecurityQuestionsService(BackendApiClient api) { _api = api; }

        public async Task<IReadOnlyList<PreguntaSeguridadDto>> GetAllAsync()
        {
            var list = await _api.GetAsync<IEnumerable<PreguntaSeguridadDto>>("api/v1/securityquestions");
            return list?.ToList() ?? new List<PreguntaSeguridadDto>();
        }

        public async Task<PreguntaSeguridadDto?> CreateAsync(string pregunta)
        {
            if (string.IsNullOrWhiteSpace(pregunta)) return null;
            var created = await _api.PostAsync<PreguntaSeguridadDto>("api/v1/securityquestions", new { Pregunta = pregunta });
            return created;
        }

        public async Task<bool> UpdateAsync(int idPregunta, string pregunta)
        {
            if (idPregunta <= 0 || string.IsNullOrWhiteSpace(pregunta)) return false;
            await _api.PutAsync<object>($"api/v1/securityquestions/{idPregunta}", new { Pregunta = pregunta });
            return true;
        }

        public async Task<bool> DeleteAsync(int idPregunta)
        {
            if (idPregunta <= 0) return false;
            await _api.DeleteAsync($"api/v1/securityquestions/{idPregunta}");
            return true;
        }

        public async Task<bool> SaveAnswersAsync(string username, Dictionary<int, string> answers)
        {
            if (string.IsNullOrWhiteSpace(username) || answers.Count == 0) return false;
            await _api.PostAsync<object>($"api/v1/securityquestions/{username}/answers", answers);
            return true;
        }
    }
}
