using Microsoft.AspNetCore.Http;

namespace AgileStockPro.Web.Services.Api;

public class TokenProvider : ITokenProvider
{
    private const string CookieName = "agilestockpro_jwt";
    private readonly IHttpContextAccessor _http;

    public TokenProvider(IHttpContextAccessor http)
    {
        _http = http;
        // Try to restore token from cookie on construction
        var ctx = _http.HttpContext;
        var fromCookie = ctx?.Request?.Cookies?[CookieName];
        if (!string.IsNullOrWhiteSpace(fromCookie))
        {
            Token = fromCookie;
        }
    }

    public string? Token { get; private set; }

    public void SetToken(string? token)
    {
        Token = token;
        // In Blazor Server interactive requests, response headers are often already sent.
        // Avoid writing cookies here to prevent "Headers are read-only" exceptions.
        var ctx = _http.HttpContext;
        if (ctx == null || ctx.Response.HasStarted)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(token))
        {
            ctx.Response.Cookies.Delete(CookieName);
        }
        else
        {
            ctx.Response.Cookies.Append(CookieName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
    }
}
