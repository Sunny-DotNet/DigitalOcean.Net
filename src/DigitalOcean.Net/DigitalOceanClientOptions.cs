namespace DigitalOcean.Net;

/// <summary>
/// Configuration options for the DigitalOcean API client.
/// DigitalOcean API 客户端的配置选项。
/// </summary>
public class DigitalOceanClientOptions
{
    /// <summary>
    /// Gets or sets the DigitalOcean API token.
    /// The token should start with <c>dop_v1_</c> (personal access token),
    /// <c>doo_v1_</c> (OAuth token), or <c>dor_v1_</c> (refresh token).
    /// <para>
    /// 获取或设置 DigitalOcean API 令牌。
    /// 令牌应以 <c>dop_v1_</c>（个人访问令牌）、<c>doo_v1_</c>（OAuth 令牌）或 <c>dor_v1_</c>（刷新令牌）开头。
    /// </para>
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base URL for the DigitalOcean API.
    /// Defaults to <c>https://api.digitalocean.com</c>.
    /// <para>获取或设置 DigitalOcean API 的基础 URL。默认为 <c>https://api.digitalocean.com</c>。</para>
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.digitalocean.com";

    /// <summary>
    /// Gets or sets the timeout for HTTP requests. Defaults to 30 seconds.
    /// <para>获取或设置 HTTP 请求的超时时间。默认为 30 秒。</para>
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the User-Agent header value. Defaults to <c>DigitalOcean.Net/{version}</c>.
    /// <para>获取或设置 User-Agent 请求头的值。默认为 <c>DigitalOcean.Net/{version}</c>。</para>
    /// </summary>
    public string UserAgent { get; set; } = $"DigitalOcean.Net/{typeof(DigitalOceanClientOptions).Assembly.GetName().Version}";
}
