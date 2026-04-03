using DigitalOcean.Net.Generated;

namespace DigitalOcean.Net.Samples;

/// <summary>
/// Sample application demonstrating DigitalOcean.Net usage.
/// 演示 DigitalOcean.Net 用法的示例应用程序。
/// </summary>
internal class Program
{
    static async Task Main(string[] args)
    {
        // Get token from environment variable
        // 从环境变量获取令牌
        var token = Environment.GetEnvironmentVariable("DIGITALOCEAN_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Please set the DIGITALOCEAN_TOKEN environment variable.");
            Console.WriteLine("请设置 DIGITALOCEAN_TOKEN 环境变量。");
            return;
        }

        // Create client / 创建客户端
        var client = DigitalOceanClientFactory.Create(token);

        Console.WriteLine("DigitalOcean.Net Sample / DigitalOcean.Net 示例");
        Console.WriteLine("=".PadRight(50, '='));

        // Example 1: Get account info / 获取账户信息
        try
        {
            Console.WriteLine("\n[1] Getting account info... / 获取账户信息...");
            var account = await client.V2.Account.GetAsync();
            Console.WriteLine($"    Account email: {account?.Account?.Email}");
            Console.WriteLine($"    Status: {account?.Account?.Status}");
            Console.WriteLine($"    Droplet limit: {account?.Account?.DropletLimit}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Error: {ex.Message}");
        }

        // Example 2: List droplets / 列出所有 Droplets
        try
        {
            Console.WriteLine("\n[2] Listing droplets... / 列出 Droplets...");
            var droplets = await client.V2.Droplets.GetAsync();
            if (droplets?.Droplets != null)
            {
                foreach (var droplet in droplets.Droplets)
                {
                    Console.WriteLine($"    - {droplet.Name} (ID: {droplet.Id}, Status: {droplet.Status})");
                }

                if (droplets.Droplets.Count == 0)
                {
                    Console.WriteLine("    No droplets found. / 未找到 Droplets。");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Error: {ex.Message}");
        }

        // Example 3: List regions / 列出可用区域
        try
        {
            Console.WriteLine("\n[3] Listing regions... / 列出可用区域...");
            var regions = await client.V2.Regions.GetAsync();
            if (regions?.Regions != null)
            {
                foreach (var region in regions.Regions.Take(5))
                {
                    Console.WriteLine($"    - {region.Name} ({region.Slug}) - Available: {region.Available}");
                }

                Console.WriteLine($"    ... and {Math.Max(0, regions.Regions.Count - 5)} more.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Error: {ex.Message}");
        }

        // Example 4: List apps / 列出应用
        try
        {
            Console.WriteLine("\n[4] Listing apps (App Platform)... / 列出应用（App Platform）...");
            var apps = await client.V2.Apps.GetAsync();
            if (apps?.Apps != null)
            {
                foreach (var app in apps.Apps)
                {
                    Console.WriteLine($"    - {app.Spec?.Name} (ID: {app.Id})");
                }

                if (apps.Apps.Count == 0)
                {
                    Console.WriteLine("    No apps found. / 未找到应用。");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Error: {ex.Message}");
        }

        Console.WriteLine("\nDone! / 完成！");
    }
}
