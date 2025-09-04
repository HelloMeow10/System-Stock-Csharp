using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Models;
using System.Collections.Generic;

namespace Presentation.ApiClient
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://localhost:5000"; // Debería estar en la configuración

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Métodos para interactuar con la API

        private async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task PostAsync(string requestUri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task PutAsync(string requestUri, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task DeleteAsync(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
        }

        // Auth
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<LoginResponse> Validate2faAsync(Validate2faRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/validate-2fa", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Users
        public async Task<List<UserDto>> GetUsersAsync() => await GetAsync<List<UserDto>>("api/users");
        public async Task<UserDto> GetUserByUsernameAsync(string username) => await GetAsync<UserDto>($"api/users/{username}");
        public async Task CreateUserAsync(UserRequest request) => await PostAsync("api/users", request);
        public async Task UpdateUserAsync(int id, UserDto user) => await PutAsync($"api/users/{id}", user);
        public async Task DeleteUserAsync(int id) => await DeleteAsync($"api/users/{id}");

        // Personas
        public async Task<List<PersonaDto>> GetPersonasAsync() => await GetAsync<List<PersonaDto>>("api/personas");
        public async Task<PersonaDto> GetPersonaAsync(int id) => await GetAsync<PersonaDto>($"api/personas/{id}");
        public async Task CreatePersonaAsync(PersonaRequest request) => await PostAsync("api/personas", request);
        public async Task UpdatePersonaAsync(int id, PersonaDto persona) => await PutAsync($"api/personas/{id}", persona);
        public async Task DeletePersonaAsync(int id) => await DeleteAsync($"api/personas/{id}");

        // Security Policy
        public async Task<PoliticaSeguridadDto> GetSecurityPolicyAsync() => await GetAsync<PoliticaSeguridadDto>("api/securitypolicy");
        public async Task UpdateSecurityPolicyAsync(PoliticaSeguridadDto policy) => await PutAsync("api/securitypolicy", policy);

        // Reference Data
        public async Task<List<TipoDocDto>> GetTiposDocAsync() => await GetAsync<List<TipoDocDto>>("api/referencedata/tiposdoc");
        public async Task<List<ProvinciaDto>> GetProvinciasAsync() => await GetAsync<List<ProvinciaDto>>("api/referencedata/provincias");
        public async Task<List<PartidoDto>> GetPartidosAsync(int provinciaId) => await GetAsync<List<PartidoDto>>($"api/referencedata/partidos/{provinciaId}");
        public async Task<List<LocalidadDto>> GetLocalidadesAsync(int partidoId) => await GetAsync<List<LocalidadDto>>($"api/referencedata/localidades/{partidoId}");
        public async Task<List<GeneroDto>> GetGenerosAsync() => await GetAsync<List<GeneroDto>>("api/referencedata/generos");
        public async Task<List<RolDto>> GetRolesAsync() => await GetAsync<List<RolDto>>("api/referencedata/roles");

        // Password
        public async Task ChangePasswordAsync(ChangePasswordRequest request) => await PostAsync("api/password/change", request);
        public async Task RecoverPasswordAsync(RecoverPasswordRequest request) => await PostAsync("api/password/recover", request);

        // Security Questions
        public async Task<List<PreguntaSeguridadDto>> GetUserSecurityQuestionsAsync(string username) => await GetAsync<List<PreguntaSeguridadDto>>($"api/securityquestions/{username}");
        public async Task<List<PreguntaSeguridadDto>> GetSecurityQuestionsAsync() => await GetAsync<List<PreguntaSeguridadDto>>("api/securityquestions");
        public async Task SaveUserSecurityAnswersAsync(string username, Dictionary<int, string> answers) => await PostAsync($"api/securityquestions/{username}/answers", answers);
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Rol { get; set; }
        public bool Requires2fa { get; set; }
    }
}
