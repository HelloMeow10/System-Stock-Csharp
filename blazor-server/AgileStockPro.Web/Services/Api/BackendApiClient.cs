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
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(content) ? $"{(int)res.StatusCode} {res.ReasonPhrase}" : content);
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
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(content) ? $"{(int)res.StatusCode} {res.ReasonPhrase}" : content);
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
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(content) ? $"{(int)res.StatusCode} {res.ReasonPhrase}" : content);
        }
    }

    public async Task DeleteAsync(string uri)
    {
        AddAuth();
        var res = await _http.DeleteAsync(uri);
        if (!res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(content) ? $"{(int)res.StatusCode} {res.ReasonPhrase}" : content);
        }
    }
}
