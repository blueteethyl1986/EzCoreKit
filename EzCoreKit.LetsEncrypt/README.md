# EzCoreKit.LetsEncrypt
針對ASP.net Core提供程序內取得Let's Encrypt服務流程。

### 安裝
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.LetsEncrypt)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.LetsEncrypt)
```
PM> Install-Package EzCoreKit.LetsEncrypt
```

### 快速上手
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