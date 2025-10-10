namespace Session
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, string? role = null);
    }
}
