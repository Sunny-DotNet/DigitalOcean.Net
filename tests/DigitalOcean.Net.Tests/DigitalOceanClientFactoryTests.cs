using DigitalOcean.Net.Generated;
using Xunit;

namespace DigitalOcean.Net.Tests;

public class DigitalOceanClientFactoryTests
{
    [Fact]
    public void Create_WithToken_ShouldReturnClient()
    {
        var client = DigitalOceanClientFactory.Create("dop_v1_test_token");

        Assert.NotNull(client);
        Assert.IsType<DigitalOceanApiClient>(client);
    }

    [Fact]
    public void Create_WithOptions_ShouldReturnClient()
    {
        var options = new DigitalOceanClientOptions
        {
            Token = "dop_v1_test_token",
            Timeout = TimeSpan.FromSeconds(60),
            BaseUrl = "https://api.digitalocean.com"
        };

        var client = DigitalOceanClientFactory.Create(options);

        Assert.NotNull(client);
        Assert.IsType<DigitalOceanApiClient>(client);
    }

    [Fact]
    public void Create_WithNullOptions_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() =>
            DigitalOceanClientFactory.Create((DigitalOceanClientOptions)null!));
    }

    [Fact]
    public void Create_WithHttpClient_ShouldReturnClient()
    {
        using var httpClient = new HttpClient();
        var client = DigitalOceanClientFactory.Create("dop_v1_test_token", httpClient);

        Assert.NotNull(client);
        Assert.IsType<DigitalOceanApiClient>(client);
    }

    [Fact]
    public void Create_WithEmptyToken_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            DigitalOceanClientFactory.Create(""));
    }
}
