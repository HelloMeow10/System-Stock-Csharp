namespace AgileStockPro.Web.Services.Api;

public interface ITokenProvider
{
    string? Token { get; }
    Task SetTokenAsync(string? token);
}
