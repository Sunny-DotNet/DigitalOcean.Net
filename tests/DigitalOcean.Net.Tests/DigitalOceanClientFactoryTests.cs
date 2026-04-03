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

    [Fact]
    public void CreateFromConfigFile_WithValidYaml_ShouldReturnClient()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "access-token: dop_v1_test_token_from_yaml\ncontext: default\n");

            var client = DigitalOceanClientFactory.CreateFromConfigFile(tempFile);

            Assert.NotNull(client);
            Assert.IsType<DigitalOceanApiClient>(client);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CreateFromConfigFile_WithConfigureOptions_ShouldApplyOptions()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "access-token: dop_v1_test_token\n");

            var client = DigitalOceanClientFactory.CreateFromConfigFile(tempFile, options =>
            {
                options.Timeout = TimeSpan.FromMinutes(5);
            });

            Assert.NotNull(client);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CreateFromConfigFile_WithMissingFile_ShouldThrow()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
            DigitalOceanClientFactory.CreateFromConfigFile(@"C:\nonexistent\config.yaml"));

        Assert.Contains("doctl config file not found", ex.Message);
    }

    [Fact]
    public void CreateFromConfigFile_WithEmptyPath_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            DigitalOceanClientFactory.CreateFromConfigFile(""));
    }

    [Fact]
    public void CreateFromConfigFile_WithMissingToken_ShouldThrow()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "context: default\n");

            var ex = Assert.Throws<InvalidOperationException>(() =>
                DigitalOceanClientFactory.CreateFromConfigFile(tempFile));

            Assert.Contains("access-token", ex.Message);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CreateFromConfigFile_WithEmptyToken_ShouldThrow()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "access-token: \ncontext: default\n");

            Assert.Throws<InvalidOperationException>(() =>
                DigitalOceanClientFactory.CreateFromConfigFile(tempFile));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void CreateFromAppData_WithNoDoctl_ShouldThrow()
    {
        // This test verifies the method throws when doctl is not installed.
        // In CI environments, doctl config typically doesn't exist.
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var configPath = Path.Combine(appData, "doctl", "config.yaml");

        if (!File.Exists(configPath))
        {
            Assert.Throws<FileNotFoundException>(() =>
                DigitalOceanClientFactory.CreateFromAppData());
        }
        else
        {
            // If doctl is installed, just verify it returns a client
            var client = DigitalOceanClientFactory.CreateFromAppData();
            Assert.NotNull(client);
        }
    }
}
