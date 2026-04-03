using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace DigitalOcean.Net.Authentication;

/// <summary>
/// An authentication provider for DigitalOcean API that uses Bearer token authentication.
/// 使用 Bearer 令牌认证的 DigitalOcean API 认证提供者。
/// </summary>
public class DigitalOceanAuthProvider : IAuthenticationProvider
{
    private readonly string _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="DigitalOceanAuthProvider"/> class.
    /// 初始化 <see cref="DigitalOceanAuthProvider"/> 类的新实例。
    /// </summary>
    /// <param name="token">The DigitalOcean API token. / DigitalOcean API 令牌。</param>
    /// <exception cref="ArgumentException">Thrown when token is null or empty. / 当令牌为 null 或空时抛出。</exception>
    public DigitalOceanAuthProvider(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or empty. / 令牌不能为 null 或空。", nameof(token));
        }

        _token = token;
    }

    /// <inheritdoc/>
    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Headers.Add("Authorization", $"Bearer {_token}");
        return Task.CompletedTask;
    }
}
