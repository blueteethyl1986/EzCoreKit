EzCoreKit.LetsEncrypt
=====
[![xpy MyGet Build Status](https://www.myget.org/BuildSource/Badge/xpy?identifier=9e998e97-5cd7-475e-bf52-1c1ffed913f4)](https://www.myget.org/)

Using Let's Encrypt on Kestrel Server.

簡單的在Kestrel Server上加入Let's Encrypt的SSL服務。

### Install Packages(安裝)
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.Markdown)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.Markdown)
```
PM> Install-Package EzCoreKit.LetsEncrypt
```

### Get Started(快速上手)
```csharp
using EzCoreKit.LetsEncrypt;
...(something)...
public class Program{
    ...(something)...
    static IWebHost host;
    public static void Main(string[] args) {
        StartServer();
    }
    public static void StartServer() {
        host = new WebHostBuilder()
        .UseKestrel(options => {
            options.UseLetsEncryptAndSave(
            savePassword: "pfxpassword",
            email: "example01@gmail.com",
            domains: "example01.gofa.tw".split(','));
            options.CheckCertificateExpired("pfxpassword", TimeSpan.FromMinutes(30), OnCertificateExpired);
        })
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseUrls("https://*", "http://*")
        .UseIISIntegration()
        .UseStartup<Startup>()
        .UseApplicationInsights()
        .Build();
        host.Run();
    }
    public static void OnCertificateExpired() {
        host.Services.GetService<IApplicationLifetime>().StopApplication();
        host.Dispose();
        StartServer();
    }
    ...(something)...
}
```