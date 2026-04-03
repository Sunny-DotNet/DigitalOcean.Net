using DigitalOcean.Net.Authentication;
using DigitalOcean.Net.Generated;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DigitalOcean.Net;

/// <summary>
/// Factory class for creating DigitalOcean API clients.
/// 用于创建 DigitalOcean API 客户端的工厂类。
/// </summary>
/// <example>
/// <code>
/// // Simple usage / 简单用法
/// var client = DigitalOceanClientFactory.Create("dop_v1_your_token");
/// var droplets = await client.V2.Droplets.GetAsync();
///
/// // With options / 使用选项
/// var client = DigitalOceanClientFactory.Create(new DigitalOceanClientOptions
/// {
///     Token = "dop_v1_your_token",
///     Timeout = TimeSpan.FromSeconds(60)
/// });
/// </code>
/// </example>
public static class DigitalOceanClientFactory
{
    /// <summary>
    /// Creates a new <see cref="DigitalOceanApiClient"/> with the specified API token.
    /// 使用指定的 API 令牌创建一个新的 <see cref="DigitalOceanApiClient"/>。
    /// </summary>
    /// <param name="token">The DigitalOcean API token. / DigitalOcean API 令牌。</param>
    /// <returns>A configured <see cref="DigitalOceanApiClient"/> instance. / 已配置的 <see cref="DigitalOceanApiClient"/> 实例。</returns>
    public static DigitalOceanApiClient Create(string token)
    {
        return Create(new DigitalOceanClientOptions { Token = token });
    }

    /// <summary>
    /// Creates a new <see cref="DigitalOceanApiClient"/> with the specified options.
    /// 使用指定的选项创建一个新的 <see cref="DigitalOceanApiClient"/>。
    /// </summary>
    /// <param name="options">The client configuration options. / 客户端配置选项。</param>
    /// <returns>A configured <see cref="DigitalOceanApiClient"/> instance. / 已配置的 <see cref="DigitalOceanApiClient"/> 实例。</returns>
    /// <exception cref="ArgumentNullException">Thrown when options is null. / 当选项为 null 时抛出。</exception>
    public static DigitalOceanApiClient Create(DigitalOceanClientOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var authProvider = new DigitalOceanAuthProvider(options.Token);
        var httpClient = CreateHttpClient(options);
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
        {
            BaseUrl = options.BaseUrl
        };

        return new DigitalOceanApiClient(adapter);
    }

    /// <summary>
    /// Creates a new <see cref="DigitalOceanApiClient"/> with a pre-configured <see cref="HttpClient"/>.
    /// Useful for advanced scenarios such as custom message handlers or proxies.
    /// 使用预配置的 <see cref="HttpClient"/> 创建新的 <see cref="DigitalOceanApiClient"/>。
    /// 适用于自定义消息处理程序或代理等高级场景。
    /// </summary>
    /// <param name="token">The DigitalOcean API token. / DigitalOcean API 令牌。</param>
    /// <param name="httpClient">The pre-configured HttpClient. / 预配置的 HttpClient。</param>
    /// <param name="baseUrl">The base URL. Defaults to <c>https://api.digitalocean.com</c>. / 基础 URL。</param>
    /// <returns>A configured <see cref="DigitalOceanApiClient"/> instance. / 已配置的 <see cref="DigitalOceanApiClient"/> 实例。</returns>
    public static DigitalOceanApiClient Create(string token, HttpClient httpClient, string baseUrl = "https://api.digitalocean.com")
    {
        var authProvider = new DigitalOceanAuthProvider(token);
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
        {
            BaseUrl = baseUrl
        };

        return new DigitalOceanApiClient(adapter);
    }

    private static HttpClient CreateHttpClient(DigitalOceanClientOptions options)
    {
        var httpClient = new HttpClient
        {
            Timeout = options.Timeout,
        };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);

        return httpClient;
    }
}
