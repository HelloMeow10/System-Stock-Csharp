namespace AgileStockPro.Web.Services.Api;

public class TokenProvider : ITokenProvider
{
    public string? Token { get; private set; }
    public void SetToken(string? token) => Token = token;
}
