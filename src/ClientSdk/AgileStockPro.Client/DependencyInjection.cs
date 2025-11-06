using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgileStockPro.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddAgileStockProClient(this IServiceCollection services, IConfiguration config, string configSection = "Backend")
    {
        var section = config.GetSection(configSection);
        var baseUrl = section["BaseUrl"] ?? "http://localhost:5000";
        return services.AddAgileStockProClient(options => options.BaseAddress = new Uri(baseUrl));
    }

    public static IServiceCollection AddAgileStockProClient(this IServiceCollection services, Action<AgileStockProClientOptions> configure)
    {
        var options = new AgileStockProClientOptions();
        configure(options);
        services.AddHttpClient<AgileStockProClient>(client =>
        {
            client.BaseAddress = options.BaseAddress ?? new Uri("http://localhost:5000");
            if (!string.IsNullOrWhiteSpace(options.ApiKeyHeader) && !string.IsNullOrWhiteSpace(options.ApiKeyValue))
            {
                client.DefaultRequestHeaders.Add(options.ApiKeyHeader, options.ApiKeyValue);
            }
        });
        return services;
    }
}

public sealed class AgileStockProClientOptions
{
    public Uri? BaseAddress { get; set; }
    public string? ApiKeyHeader { get; set; }
    public string? ApiKeyValue { get; set; }
}
