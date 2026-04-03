using DigitalOcean.Net.Authentication;
using Microsoft.Kiota.Abstractions;
using Xunit;

namespace DigitalOcean.Net.Tests;

public class DigitalOceanAuthProviderTests
{
    [Fact]
    public void Constructor_WithValidToken_ShouldNotThrow()
    {
        var provider = new DigitalOceanAuthProvider("dop_v1_test_token");
        Assert.NotNull(provider);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidToken_ShouldThrow(string? token)
    {
        Assert.Throws<ArgumentException>(() => new DigitalOceanAuthProvider(token!));
    }

    [Fact]
    public async Task AuthenticateRequestAsync_ShouldAddBearerToken()
    {
        const string token = "dop_v1_test_token";
        var provider = new DigitalOceanAuthProvider(token);
        var request = new RequestInformation
        {
            HttpMethod = Method.GET,
            URI = new Uri("https://api.digitalocean.com/v2/droplets")
        };

        await provider.AuthenticateRequestAsync(request);

        Assert.Contains("Authorization", request.Headers.Keys);
    }

    [Fact]
    public async Task AuthenticateRequestAsync_WithNullRequest_ShouldThrow()
    {
        var provider = new DigitalOceanAuthProvider("dop_v1_test");
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            provider.AuthenticateRequestAsync(null!));
    }
}
