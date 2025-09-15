using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Services.Tests.Helpers;
using System;

namespace Services.Tests
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        public Action<IServiceCollection> TestServices { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real authentication and add the test one
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });

                // Allow tests to register their own mock services
                TestServices?.Invoke(services);
            });
        }
    }
}
