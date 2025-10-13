namespace AgileStockPro.Web.Services.Api;

public interface ITokenProvider
{
    string? Token { get; }
    void SetToken(string? token);
}
