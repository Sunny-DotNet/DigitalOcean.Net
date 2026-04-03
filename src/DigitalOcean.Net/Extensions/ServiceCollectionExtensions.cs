using DigitalOcean.Net.Authentication;
using DigitalOcean.Net.Generated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DigitalOcean.Net.Extensions;

/// <summary>
/// Extension methods for registering DigitalOcean API client services with the dependency injection container.
/// 用于在依赖注入容器中注册 DigitalOcean API 客户端服务的扩展方法。
/// </summary>
/// <example>
/// <code>
/// // In your Program.cs or Startup.cs / 在 Program.cs 或 Startup.cs 中
/// services.AddDigitalOceanClient(options =>
/// {
///     options.Token = configuration["DigitalOcean:Token"]!;
/// });
///
/// // Then inject the client / 然后注入客户端
/// public class MyService(DigitalOceanApiClient client)
/// {
///     public async Task DoWork()
///     {
///         var droplets = await client.V2.Droplets.GetAsync();
///     }
/// }
/// </code>
/// </example>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the DigitalOcean API client to the service collection.
    /// 将 DigitalOcean API 客户端添加到服务集合中。
    /// </summary>
    /// <param name="services">The service collection. / 服务集合。</param>
    /// <param name="configureOptions">An action to configure the client options. / 配置客户端选项的操作。</param>
    /// <returns>The service collection for chaining. / 用于链式调用的服务集合。</returns>
    public static IServiceCollection AddDigitalOceanClient(
        this IServiceCollection services,
        Action<DigitalOceanClientOptions> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.Configure(configureOptions);

        services.TryAddSingleton<DigitalOceanAuthProvider>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<DigitalOceanClientOptions>>().Value;
            return new DigitalOceanAuthProvider(options.Token);
        });

        services.AddHttpClient("DigitalOcean", (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<DigitalOceanClientOptions>>().Value;
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
        });

        services.TryAddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<DigitalOceanClientOptions>>().Value;
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("DigitalOcean");
            var authProvider = sp.GetRequiredService<DigitalOceanAuthProvider>();

            var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
            {
                BaseUrl = options.BaseUrl
            };

            return new DigitalOceanApiClient(adapter);
        });

        return services;
    }
}
