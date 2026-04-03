using DigitalOcean.Net.Authentication;
using DigitalOcean.Net.Generated;
using Microsoft.Kiota.Http.HttpClientLibrary;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
/// // From doctl config / 从 doctl 配置文件创建
/// var client = DigitalOceanClientFactory.CreateFromAppData();
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

    /// <summary>
    /// Creates a new <see cref="DigitalOceanApiClient"/> by reading the access token
    /// from the <c>doctl</c> CLI configuration file at <c>%APPDATA%\doctl\config.yaml</c> (Windows)
    /// or <c>~/.config/doctl/config.yaml</c> (Linux/macOS).
    /// <para>
    /// 从 <c>doctl</c> CLI 配置文件读取访问令牌来创建新的 <see cref="DigitalOceanApiClient"/>。
    /// 配置文件路径：Windows 为 <c>%APPDATA%\doctl\config.yaml</c>，Linux/macOS 为 <c>~/.config/doctl/config.yaml</c>。
    /// </para>
    /// </summary>
    /// <param name="configureOptions">
    /// Optional action to further configure client options (e.g., timeout, base URL).
    /// The token will be pre-populated from the config file.
    /// <para>可选的配置操作，用于进一步配置客户端选项（如超时、基础 URL）。令牌将从配置文件中预填充。</para>
    /// </param>
    /// <returns>A configured <see cref="DigitalOceanApiClient"/> instance. / 已配置的 <see cref="DigitalOceanApiClient"/> 实例。</returns>
    /// <exception cref="FileNotFoundException">Thrown when the doctl config file is not found. / 当 doctl 配置文件不存在时抛出。</exception>
    /// <exception cref="InvalidOperationException">Thrown when the access token is not found in the config file. / 当配置文件中未找到访问令牌时抛出。</exception>
    public static DigitalOceanApiClient CreateFromAppData(Action<DigitalOceanClientOptions>? configureOptions = null)
    {
        var configPath = GetDoctlConfigPath();
        return CreateFromConfigFile(configPath, configureOptions);
    }

    /// <summary>
    /// Creates a new <see cref="DigitalOceanApiClient"/> by reading the access token
    /// from a specified <c>doctl</c> configuration file.
    /// <para>
    /// 从指定的 <c>doctl</c> 配置文件读取访问令牌来创建新的 <see cref="DigitalOceanApiClient"/>。
    /// </para>
    /// </summary>
    /// <param name="configFilePath">The full path to the doctl config.yaml file. / doctl config.yaml 文件的完整路径。</param>
    /// <param name="configureOptions">
    /// Optional action to further configure client options.
    /// <para>可选的配置操作，用于进一步配置客户端选项。</para>
    /// </param>
    /// <returns>A configured <see cref="DigitalOceanApiClient"/> instance. / 已配置的 <see cref="DigitalOceanApiClient"/> 实例。</returns>
    /// <exception cref="ArgumentException">Thrown when configFilePath is null or empty. / 当配置文件路径为 null 或空时抛出。</exception>
    /// <exception cref="FileNotFoundException">Thrown when the config file is not found. / 当配置文件不存在时抛出。</exception>
    /// <exception cref="InvalidOperationException">Thrown when the access token is not found in the config file. / 当配置文件中未找到访问令牌时抛出。</exception>
    public static DigitalOceanApiClient CreateFromConfigFile(string configFilePath, Action<DigitalOceanClientOptions>? configureOptions = null)
    {
        if (string.IsNullOrWhiteSpace(configFilePath))
        {
            throw new ArgumentException(
                "Config file path cannot be null or empty. / 配置文件路径不能为 null 或空。",
                nameof(configFilePath));
        }

        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException(
                $"doctl config file not found at: {configFilePath}. " +
                $"Please install doctl and run 'doctl auth init' first. / " +
                $"doctl 配置文件未找到：{configFilePath}。请先安装 doctl 并运行 'doctl auth init'。",
                configFilePath);
        }

        var token = ReadTokenFromConfig(configFilePath);

        var options = new DigitalOceanClientOptions { Token = token };
        configureOptions?.Invoke(options);

        return Create(options);
    }

    private static string GetDoctlConfigPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "doctl", "config.yaml");
    }

    private static string ReadTokenFromConfig(string configFilePath)
    {
        var yaml = File.ReadAllText(configFilePath);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var config = deserializer.Deserialize<DoctlConfig>(yaml);

        if (config == null || string.IsNullOrWhiteSpace(config.AccessToken))
        {
            throw new InvalidOperationException(
                $"No 'access-token' found in doctl config file: {configFilePath}. " +
                $"Please run 'doctl auth init' to configure your token. / " +
                $"在 doctl 配置文件中未找到 'access-token'：{configFilePath}。请运行 'doctl auth init' 配置令牌。");
        }

        return config.AccessToken;
    }

    private sealed class DoctlConfig
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? Context { get; set; }
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
