using Microsoft.AspNetCore.Authentication;

namespace Services.Authentication
{
    public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "ApiKey";
        public string HeaderName { get; set; } = "X-Api-Key";
    }
}
