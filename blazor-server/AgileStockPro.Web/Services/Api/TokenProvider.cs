using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace AgileStockPro.Web.Services.Api;

public class TokenProvider : ITokenProvider
{
    private const string CookieName = "ums_auth";
    private readonly IHttpContextAccessor _http;
    private readonly IJSRuntime _js;

    public TokenProvider(IHttpContextAccessor http, IJSRuntime js)
    {
        _http = http;
        _js = js;
        // Try to restore token from cookie on construction
        var ctx = _http.HttpContext;
        var fromCookie = ctx?.Request?.Cookies?[CookieName];
        if (!string.IsNullOrWhiteSpace(fromCookie))
        {
            Token = fromCookie;
        }
    }

    public string? Token { get; private set; }

    public async Task SetTokenAsync(string? token)
    {
        Token = token;
        // In Blazor Server interactive requests, response headers are often already sent.
        // We use JS Interop to set the cookie in the browser.
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                await _js.InvokeVoidAsync("app.setCookie", CookieName, "", -1);
            }
            else
            {
                await _js.InvokeVoidAsync("app.setCookie", CookieName, token, 7);
            }
        }
        catch
        {
            // If JS interop fails (e.g. pre-rendering), try HttpContext as fallback
            var ctx = _http.HttpContext;
            if (ctx != null && !ctx.Response.HasStarted)
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    ctx.Response.Cookies.Delete(CookieName);
                }
                else
                {
                    ctx.Response.Cookies.Append(CookieName, token, new CookieOptions
                    {
                        HttpOnly = false, // Must be accessible to JS if we want to read it back easily, or use HttpOnly=true and rely on server reading it
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }
            }
        }
    }
}
