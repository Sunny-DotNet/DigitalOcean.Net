using Xunit;

namespace DigitalOcean.Net.Tests;

public class DigitalOceanClientOptionsTests
{
    [Fact]
    public void DefaultOptions_ShouldHaveCorrectDefaults()
    {
        var options = new DigitalOceanClientOptions();

        Assert.Equal(string.Empty, options.Token);
        Assert.Equal("https://api.digitalocean.com", options.BaseUrl);
        Assert.Equal(TimeSpan.FromSeconds(30), options.Timeout);
        Assert.StartsWith("DigitalOcean.Net/", options.UserAgent);
    }

    [Fact]
    public void Options_ShouldAllowCustomization()
    {
        var options = new DigitalOceanClientOptions
        {
            Token = "dop_v1_custom",
            BaseUrl = "https://custom.api.com",
            Timeout = TimeSpan.FromMinutes(2),
            UserAgent = "MyApp/1.0"
        };

        Assert.Equal("dop_v1_custom", options.Token);
        Assert.Equal("https://custom.api.com", options.BaseUrl);
        Assert.Equal(TimeSpan.FromMinutes(2), options.Timeout);
        Assert.Equal("MyApp/1.0", options.UserAgent);
    }
}
