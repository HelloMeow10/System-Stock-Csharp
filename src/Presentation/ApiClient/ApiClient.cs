using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts;
using System.Collections.Generic;
using Presentation.Exceptions; // Add this using directive
using System.Linq;
using SharedKernel;
using Presentation.Helpers;
using System.Net;

namespace Presentation.ApiClient
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string? _csrfToken;

        public ApiClient(string baseAddress)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()
            };

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task HandleFailedResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                // No content provided; construct a reasonable message from status code
                throw new ApiException($"{(int)response.StatusCode} {response.ReasonPhrase}", response.StatusCode);
            }

            try
            {
                // Try our standard error contract first
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (errorResponse != null)
                {
                    string message = errorResponse.Message;
                    if (errorResponse.Errors != null && errorResponse.Errors.Any())
                    {
                        message = string.Join(Environment.NewLine, errorResponse.Errors);
                    }
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        throw new ApiException(message, response.StatusCode, errorResponse.Errors);
                    }
                }
            }
            catch (JsonException)
            {
                // Fall through to try ProblemDetails or raw content
            }

            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("title", out var title))
                {
                    var detail = doc.RootElement.TryGetProperty("detail", out var det) ? det.GetString() : null;
                    var titleStr = title.GetString();
                    var msg = string.IsNullOrWhiteSpace(detail) ? titleStr : $"{titleStr}: {detail}";
                    throw new ApiException(msg ?? "API error", response.StatusCode);
                }
            }
            catch (JsonException)
            {
                // Content is not JSON; use raw text
            }

            var fallbackMsg = content.Trim();
            if (string.IsNullOrWhiteSpace(fallbackMsg))
            {
                fallbackMsg = $"{(int)response.StatusCode} {response.ReasonPhrase}";
            }
            throw new ApiException(fallbackMsg, response.StatusCode);
        }

        private async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result is null)
            {
                throw new ApiException("Unexpected empty response from API.", HttpStatusCode.InternalServerError);
            }
            return result;
        }

        private async Task<T> PostAsync<T>(string requestUri, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            AddCsrfTokenToRequest(request);
            var json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result is null)
            {
                throw new ApiException("Unexpected empty response from API.", HttpStatusCode.InternalServerError);
            }
            return result;
        }

        private async Task PostAsync(string requestUri, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            AddCsrfTokenToRequest(request);
            var json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
        }

        private async Task PutAsync(string requestUri, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            AddCsrfTokenToRequest(request);
            var json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
        }

        private async Task DeleteAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            AddCsrfTokenToRequest(request);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
        }

        private async Task PatchAsync(string requestUri, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);
            AddCsrfTokenToRequest(request);
            var json = JsonSerializer.Serialize(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
        }

        private void AddCsrfTokenToRequest(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_csrfToken))
            {
                request.Headers.Add("X-CSRF-TOKEN", _csrfToken);
            }
        }

        // Auth
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = await PostAsync<LoginResponse>("api/v1/auth/login", request);
            return response;
        }

        public async Task<LoginResponse> Validate2faAsync(Validate2faRequest request)
        {
            var response = await PostAsync<LoginResponse>("api/v1/auth/validate-2fa", request);
            return response;
        }

        public async Task LogoutAsync()
        {
            await PostAsync("api/v1/auth/logout", new { });
            _csrfToken = null;
        }

        private async Task FetchAndStoreCsrfTokenAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/auth/csrf-token");
            if (!response.IsSuccessStatusCode)
            {
                await HandleFailedResponse(response);
            }
            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(json);
            _csrfToken = tokenResponse.GetProperty("token").GetString();
        }

        // Users
        public async Task<PagedResponse<UserDto>> GetUsersAsync(UserQueryParameters queryParams) => await GetAsync<PagedResponse<UserDto>>($"api/v1/users?{queryParams.ToQueryString()}");
        // Convenience overload used in UI when no params are provided
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var paged = await GetUsersAsync(new UserQueryParameters());
            return paged.Items?.ToList() ?? new List<UserDto>();
        }
        // Helper to get a single user by username via filtering users endpoint
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var result = await GetUsersAsync(new UserQueryParameters { Username = username, PageSize = 1, PageNumber = 1 });
            return result.Items?.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        }
        // Current user (non-admin friendly)
        public async Task<UserDto> GetCurrentUserAsync() => await GetAsync<UserDto>("api/v1/users/me");
        public async Task<UserDto> GetUserByIdAsync(int id) => await GetAsync<UserDto>($"api/v1/users/{id}");
        public async Task CreateUserAsync(UserRequest request) => await PostAsync("api/v1/users", request);
        public async Task UpdateUserAsync(int id, UpdateUserRequest user) => await PutAsync($"api/v1/users/{id}", user);
        public async Task PatchUserAsync(int id, object patchDoc) => await PatchAsync($"api/v1/users/{id}", patchDoc);
        public async Task DeleteUserAsync(int id) => await DeleteAsync($"api/v1/users/{id}");

        // Personas
        public async Task<PagedResponse<PersonaDto>> GetPersonasAsync(PaginationParams paginationParams) => await GetAsync<PagedResponse<PersonaDto>>($"api/v1/personas?{paginationParams.ToQueryString()}");
        // Convenience overload used in UI
        public async Task<List<PersonaDto>> GetPersonasAsync()
        {
            var paged = await GetPersonasAsync(new PaginationParams());
            return paged.Items?.ToList() ?? new List<PersonaDto>();
        }
        public async Task<PersonaDto> GetPersonaAsync(int id) => await GetAsync<PersonaDto>($"api/v1/personas/{id}");
        public async Task CreatePersonaAsync(PersonaRequest request) => await PostAsync("api/v1/personas", request);
        public async Task UpdatePersonaAsync(int id, UpdatePersonaRequest persona) => await PutAsync($"api/v1/personas/{id}", persona);
        public async Task PatchPersonaAsync(int id, object patchDoc) => await PatchAsync($"api/v1/personas/{id}", patchDoc);
        public async Task DeletePersonaAsync(int id) => await DeleteAsync($"api/v1/personas/{id}");

        // Security Policy
    public async Task<PoliticaSeguridadDto> GetSecurityPolicyAsync() => await GetAsync<PoliticaSeguridadDto>("api/v1/securitypolicy");
    public async Task UpdateSecurityPolicyAsync(UpdatePoliticaSeguridadRequest policy) => await PutAsync("api/v1/securitypolicy", policy);

        // Reference Data
        public async Task<List<TipoDocDto>> GetTiposDocAsync() => await GetAsync<List<TipoDocDto>>("api/v1/referencedata/tiposdoc");
        public async Task<List<ProvinciaDto>> GetProvinciasAsync() => await GetAsync<List<ProvinciaDto>>("api/v1/referencedata/provincias");
        public async Task<List<PartidoDto>> GetPartidosAsync(int provinciaId) => await GetAsync<List<PartidoDto>>($"api/v1/referencedata/partidos/{provinciaId}");
        public async Task<List<LocalidadDto>> GetLocalidadesAsync(int partidoId) => await GetAsync<List<LocalidadDto>>($"api/v1/referencedata/localidades/{partidoId}");
        public async Task<List<GeneroDto>> GetGenerosAsync() => await GetAsync<List<GeneroDto>>("api/v1/referencedata/generos");
        public async Task<List<RolDto>> GetRolesAsync() => await GetAsync<List<RolDto>>("api/v1/referencedata/roles");

        // Password
        public async Task ChangePasswordAsync(ChangePasswordRequest request) => await PostAsync("api/v1/password/change", request);
        public async Task RecoverPasswordAsync(RecoverPasswordRequest request) => await PostAsync("api/v1/password/recover", request);

        // Security Questions
        public async Task<List<PreguntaSeguridadDto>> GetUserSecurityQuestionsAsync(string username) => await GetAsync<List<PreguntaSeguridadDto>>($"api/v1/securityquestions/{username}");
        public async Task<List<PreguntaSeguridadDto>> GetSecurityQuestionsAsync() => await GetAsync<List<PreguntaSeguridadDto>>("api/v1/securityquestions");
        public async Task SaveUserSecurityAnswersAsync(string username, Dictionary<int, string> answers) => await PostAsync($"api/v1/securityquestions/{username}/answers", answers);
    }

}
