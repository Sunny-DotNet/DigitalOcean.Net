using DigitalOcean.Net.Extensions;
using DigitalOcean.Net.Generated;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DigitalOcean.Net.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDigitalOceanClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        services.AddDigitalOceanClient(options =>
        {
            options.Token = "dop_v1_test_token";
        });

        var provider = services.BuildServiceProvider();
        var client = provider.GetService<DigitalOceanApiClient>();

        Assert.NotNull(client);
    }

    [Fact]
    public void AddDigitalOceanClient_WithNullServices_ShouldThrow()
    {
        IServiceCollection services = null!;

        Assert.Throws<ArgumentNullException>(() =>
            services.AddDigitalOceanClient(options => { options.Token = "test"; }));
    }

    [Fact]
    public void AddDigitalOceanClient_WithNullAction_ShouldThrow()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() =>
            services.AddDigitalOceanClient(null!));
    }

    [Fact]
    public void AddDigitalOceanClient_ShouldResolveMultipleTimes()
    {
        var services = new ServiceCollection();
        services.AddDigitalOceanClient(options =>
        {
            options.Token = "dop_v1_test_token";
        });

        var provider = services.BuildServiceProvider();

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var client1 = scope1.ServiceProvider.GetService<DigitalOceanApiClient>();
        var client2 = scope2.ServiceProvider.GetService<DigitalOceanApiClient>();

        Assert.NotNull(client1);
        Assert.NotNull(client2);
        Assert.NotSame(client1, client2);
    }
}
