# DigitalOcean.Net

[ English ](README.md) | [ **中文** ](README.zh-CN.md)

---

[![NuGet](https://img.shields.io/nuget/v/DigitalOcean.Net.svg)](https://www.nuget.org/packages/DigitalOcean.Net)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/m67186636/DigitalOcean.Net/actions/workflows/ci.yml/badge.svg)](https://github.com/m67186636/DigitalOcean.Net/actions/workflows/ci.yml)

一个全面的 DigitalOcean API .NET 客户端库，覆盖**全部 40+ 资源类别**，包括 Droplets、Kubernetes、App Platform、Databases 等。基于 [DigitalOcean 官方 OpenAPI 规范](https://github.com/digitalocean/openapi) 自动生成。

## 功能特性

- ✅ **全量 API 覆盖** — 40+ DigitalOcean 资源类别（Droplets、Apps、Databases、Kubernetes、VPCs 等）
- ✅ **多目标框架** — 支持 `netstandard2.0`、`netstandard2.1`、`net8.0`、`net9.0`、`net10.0`
- ✅ **强类型** — 从 [官方 OpenAPI 规范](https://github.com/digitalocean/openapi) 自动生成
- ✅ **依赖注入** — 一流的 `Microsoft.Extensions.DependencyInjection` 支持
- ✅ **CancellationToken** — 完整的异步/等待与取消支持
- ✅ **SourceLink** — 支持调试时步入库源代码
- ✅ **doctl 集成** — 直接从 `doctl` CLI 配置文件读取令牌

## 安装

```bash
dotnet add package DigitalOcean.Net
```

## 快速开始

### 直接使用

```csharp
using DigitalOcean.Net;

// 使用 API 令牌创建客户端
var client = DigitalOceanClientFactory.Create("dop_v1_your_token_here");

// 或从 doctl CLI 配置文件读取令牌（%APPDATA%\doctl\config.yaml）
var client = DigitalOceanClientFactory.CreateFromAppData();

// 列出所有 Droplets
var dropletsResponse = await client.V2.Droplets.GetAsync();
foreach (var droplet in dropletsResponse?.Droplets ?? [])
{
    Console.WriteLine($"{droplet.Name} - {droplet.Status}");
}

// 获取账户信息
var account = await client.V2.Account.GetAsync();
Console.WriteLine($"Email: {account?.Account?.Email}");

// 列出应用（App Platform）
var apps = await client.V2.Apps.GetAsync();

// 列出 Kubernetes 集群
var k8s = await client.V2.Kubernetes.Clusters.GetAsync();

// 列出数据库
var dbs = await client.V2.Databases.GetAsync();
```

### 使用依赖注入

```csharp
using DigitalOcean.Net.Extensions;

// 在 Program.cs 或 Startup.cs 中
builder.Services.AddDigitalOceanClient(options =>
{
    options.Token = builder.Configuration["DigitalOcean:Token"]!;
});

// 在服务类中注入使用
public class MyService(DigitalOceanApiClient client)
{
    public async Task ListDroplets()
    {
        var response = await client.V2.Droplets.GetAsync();
        // ...
    }
}
```

### 高级配置

```csharp
// 自定义选项
var client = DigitalOceanClientFactory.Create(new DigitalOceanClientOptions
{
    Token = "dop_v1_your_token_here",
    BaseUrl = "https://api.digitalocean.com",
    Timeout = TimeSpan.FromSeconds(60),
    UserAgent = "MyApp/1.0"
});

// 使用自定义 HttpClient（适用于代理、自定义处理程序等）
var httpClient = new HttpClient(new MyCustomHandler());
var client = DigitalOceanClientFactory.Create("dop_v1_token", httpClient);
```

## API 覆盖范围

| 类别 | 描述 | 命名空间 |
|---|---|---|
| Account | 账户信息 | `client.V2.Account` |
| Actions | 操作记录 | `client.V2.Actions` |
| Apps | 应用平台 | `client.V2.Apps` |
| Billing | 账单与发票 | `client.V2.Customers` |
| CDN | 内容分发 | `client.V2.Cdn` |
| Certificates | SSL 证书 | `client.V2.Certificates` |
| Databases | 托管数据库 | `client.V2.Databases` |
| Domains | DNS 管理 | `client.V2.Domains` |
| Droplets | 虚拟机 | `client.V2.Droplets` |
| Firewalls | 云防火墙 | `client.V2.Firewalls` |
| Functions | Serverless 函数 | `client.V2.Functions` |
| Images | 系统镜像 | `client.V2.Images` |
| Kubernetes | K8s 集群 | `client.V2.Kubernetes` |
| Load Balancers | 负载均衡器 | `client.V2.LoadBalancers` |
| Monitoring | 监控告警 | `client.V2.Monitoring` |
| Projects | 项目管理 | `client.V2.Projects` |
| Regions | 可用区域 | `client.V2.Regions` |
| Registry | 容器镜像仓库 | `client.V2.Registry` |
| Reserved IPs | 保留 IP | `client.V2.ReservedIps` |
| Sizes | 规格 | `client.V2.Sizes` |
| Snapshots | 快照 | `client.V2.Snapshots` |
| Spaces | 对象存储 | `client.V2.Spaces` |
| SSH Keys | SSH 密钥管理 | `client.V2.Account` |
| Tags | 资源标签 | `client.V2.Tags` |
| Uptime | 可用性检测 | `client.V2.Uptime` |
| Volumes | 块存储 | `client.V2.Volumes` |
| VPCs | 虚拟私有网络 | `client.V2.Vpcs` |

## 认证

DigitalOcean 使用 OAuth Bearer 令牌认证。从 [DigitalOcean 控制面板](https://cloud.digitalocean.com/account/api/tokens) 生成令牌。

令牌前缀：
- `dop_v1_` — 个人访问令牌
- `doo_v1_` — OAuth 应用令牌
- `dor_v1_` — 刷新令牌

## 系统要求

- .NET Standard 2.0+ / .NET 8.0+ / .NET 9.0+ / .NET 10.0+
- DigitalOcean 账户和 API 令牌

## 贡献

欢迎贡献！请创建 Issue 或提交 Pull Request。

## 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。
