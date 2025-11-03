using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AgileStockPro.Web.Services.Api;

public class BackendApiClient
{
    private readonly HttpClient _http;
    private readonly ITokenProvider _tokens;
    private static readonly JsonSerializerOptions _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public BackendApiClient(HttpClient http, ApiOptions opts, ITokenProvider tokens)
    {
        _http = http;
        _http.BaseAddress = new Uri(opts.BaseUrl);
        _tokens = tokens;
    }

    private void AddAuth()
    {
        _http.DefaultRequestHeaders.Authorization = null;
        // Prevent caching of API responses to ensure fresh policy/user data
        _http.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true, MustRevalidate = true };
        if (!string.IsNullOrWhiteSpace(_tokens.Token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.Token);
        }
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        AddAuth();
        var res = await _http.GetAsync(uri);
        var content = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            throw ApiException.FromResponse(res.StatusCode, content, res.ReasonPhrase);
        }
        var obj = JsonSerializer.Deserialize<T>(content, _json);
        if (obj == null) throw new InvalidOperationException("Respuesta vacía del servidor");
        return obj;
    }

    public async Task<T> PostAsync<T>(string uri, object body)
    {
        AddAuth();
        var json = JsonSerializer.Serialize(body);
        var res = await _http.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
        var content = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            throw ApiException.FromResponse(res.StatusCode, content, res.ReasonPhrase);
        }
        var obj = JsonSerializer.Deserialize<T>(content, _json);
        if (obj == null) throw new InvalidOperationException("Respuesta vacía del servidor");
        return obj;
    }

    public async Task PostAsync(string uri, object body)
    {
        AddAuth();
        var json = JsonSerializer.Serialize(body);
        var res = await _http.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
        if (!res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            throw ApiException.FromResponse(res.StatusCode, content, res.ReasonPhrase);
        }
    }

    public async Task<T> PutAsync<T>(string uri, object body)
    {
        AddAuth();
        var json = JsonSerializer.Serialize(body);
        var res = await _http.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
        var content = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            throw ApiException.FromResponse(res.StatusCode, content, res.ReasonPhrase);
        }
        var obj = JsonSerializer.Deserialize<T>(content, _json);
        if (obj == null) throw new InvalidOperationException("Respuesta vacía del servidor");
        return obj;
    }

    public async Task DeleteAsync(string uri)
    {
        AddAuth();
        var res = await _http.DeleteAsync(uri);
        if (!res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            throw ApiException.FromResponse(res.StatusCode, content, res.ReasonPhrase);
        }
    }
}

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? Title { get; }
    public string? Detail { get; }

    public ApiException(int statusCode, string? message, string? title = null, string? detail = null) : base(message)
    {
        StatusCode = statusCode;
        Title = title;
        Detail = detail;
    }

    public static ApiException FromResponse(System.Net.HttpStatusCode statusCode, string? content, string? reason)
    {
        // Try parse ProblemDetails
        string? title = null; string? detail = null; string message = string.Empty;
        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("title", out var t) && t.ValueKind == JsonValueKind.String) title = t.GetString();
                if (doc.RootElement.TryGetProperty("detail", out var d) && d.ValueKind == JsonValueKind.String) detail = d.GetString();
            }
            catch
            {
                // not JSON; fall through
            }
        }
        message = detail ?? title ?? content ?? $"{(int)statusCode} {reason}";
        return new ApiException((int)statusCode, message, title, detail);
    }
}
