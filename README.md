# DigitalOcean.Net

[ **English** ](README.md) | [ 中文 ](README.zh-CN.md)

---

[![NuGet](https://img.shields.io/nuget/v/DigitalOcean.Net.svg)](https://www.nuget.org/packages/DigitalOcean.Net)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/m67186636/DigitalOcean.Net/actions/workflows/ci.yml/badge.svg)](https://github.com/m67186636/DigitalOcean.Net/actions/workflows/ci.yml)

A comprehensive .NET client library for the [DigitalOcean API](https://docs.digitalocean.com/reference/api/reference/), covering **all 40+ resource categories** including Droplets, Kubernetes, App Platform, Databases, and more.

## Features

- ✅ **Full API Coverage** — All 40+ DigitalOcean resource categories (Droplets, Apps, Databases, Kubernetes, VPCs, etc.)
- ✅ **Multi-targeting** — Supports `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0`, `net10.0`
- ✅ **Strongly-typed** — Auto-generated from the [official OpenAPI specification](https://github.com/digitalocean/openapi)
- ✅ **Dependency Injection** — First-class support for `Microsoft.Extensions.DependencyInjection`
- ✅ **CancellationToken** — Full async/await with cancellation support
- ✅ **SourceLink** — Debug into library source code
- ✅ **doctl Integration** — Read token directly from `doctl` CLI config

## Installation

```bash
dotnet add package DigitalOcean.Net
```

## Quick Start

### Direct Usage

```csharp
using DigitalOcean.Net;

// Create client with your API token
var client = DigitalOceanClientFactory.Create("dop_v1_your_token_here");

// Or read token from doctl CLI config (~/.config/doctl/config.yaml)
var client = DigitalOceanClientFactory.CreateFromAppData();

// List all Droplets
var dropletsResponse = await client.V2.Droplets.GetAsync();
foreach (var droplet in dropletsResponse?.Droplets ?? [])
{
    Console.WriteLine($"{droplet.Name} - {droplet.Status}");
}

// Get account info
var account = await client.V2.Account.GetAsync();
Console.WriteLine($"Email: {account?.Account?.Email}");

// List Apps (App Platform)
var apps = await client.V2.Apps.GetAsync();

// List Kubernetes clusters
var k8s = await client.V2.Kubernetes.Clusters.GetAsync();

// List databases
var dbs = await client.V2.Databases.GetAsync();
```

### With Dependency Injection

```csharp
using DigitalOcean.Net.Extensions;

// In Program.cs or Startup.cs
builder.Services.AddDigitalOceanClient(options =>
{
    options.Token = builder.Configuration["DigitalOcean:Token"]!;
});

// In your service class
public class MyService(DigitalOceanApiClient client)
{
    public async Task ListDroplets()
    {
        var response = await client.V2.Droplets.GetAsync();
        // ...
    }
}
```

### Advanced Configuration

```csharp
// Custom options
var client = DigitalOceanClientFactory.Create(new DigitalOceanClientOptions
{
    Token = "dop_v1_your_token_here",
    BaseUrl = "https://api.digitalocean.com",
    Timeout = TimeSpan.FromSeconds(60),
    UserAgent = "MyApp/1.0"
});

// Use your own HttpClient (for proxies, custom handlers, etc.)
var httpClient = new HttpClient(new MyCustomHandler());
var client = DigitalOceanClientFactory.Create("dop_v1_token", httpClient);
```

## API Coverage

| Category | Description | Namespace |
|---|---|---|
| Account | Account info | `client.V2.Account` |
| Actions | Action history | `client.V2.Actions` |
| Apps | App Platform | `client.V2.Apps` |
| Billing | Billing & invoices | `client.V2.Customers` |
| CDN | Content delivery | `client.V2.Cdn` |
| Certificates | SSL certificates | `client.V2.Certificates` |
| Databases | Managed databases | `client.V2.Databases` |
| Domains | DNS management | `client.V2.Domains` |
| Droplets | Virtual machines | `client.V2.Droplets` |
| Firewalls | Cloud firewalls | `client.V2.Firewalls` |
| Functions | Serverless functions | `client.V2.Functions` |
| Images | OS images | `client.V2.Images` |
| Kubernetes | K8s clusters | `client.V2.Kubernetes` |
| Load Balancers | Load balancers | `client.V2.LoadBalancers` |
| Monitoring | Alerts & metrics | `client.V2.Monitoring` |
| Projects | Project management | `client.V2.Projects` |
| Regions | Available regions | `client.V2.Regions` |
| Registry | Container registry | `client.V2.Registry` |
| Reserved IPs | Static IPs | `client.V2.ReservedIps` |
| Sizes | Droplet sizes | `client.V2.Sizes` |
| Snapshots | Volume snapshots | `client.V2.Snapshots` |
| Spaces | Object storage | `client.V2.Spaces` |
| SSH Keys | SSH key management | `client.V2.Account` |
| Tags | Resource tagging | `client.V2.Tags` |
| Uptime | Uptime checks | `client.V2.Uptime` |
| Volumes | Block storage | `client.V2.Volumes` |
| VPCs | Virtual networks | `client.V2.Vpcs` |

## Authentication

DigitalOcean uses OAuth Bearer tokens. Generate a token from the [DigitalOcean Control Panel](https://cloud.digitalocean.com/account/api/tokens).

Token prefixes:
- `dop_v1_` — Personal access token
- `doo_v1_` — OAuth application token
- `dor_v1_` — Refresh token

## Requirements

- .NET Standard 2.0+ / .NET 8.0+ / .NET 9.0+ / .NET 10.0+
- A DigitalOcean account and API token

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.