using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Services.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Missing header: {Options.HeaderName}"));
            }

            var providedApiKey = apiKeyHeaderValues.ToString();
            var configuredApiKey = _configuration["Authentication:ApiKey"];

            if (string.IsNullOrWhiteSpace(providedApiKey) || !providedApiKey.Equals(configuredApiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key provided."));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "ApiKeyUser")
                // You can add more claims here if needed, like roles or permissions
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
