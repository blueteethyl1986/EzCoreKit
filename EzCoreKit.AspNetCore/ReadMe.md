EzCoreKit.AspNetCore
=====
[![xpy MyGet Build Status](https://www.myget.org/BuildSource/Badge/xpy?identifier=9e998e97-5cd7-475e-bf52-1c1ffed913f4)](https://www.myget.org/)

Extension Methods & Factory Class for ASP.net Core

針對ASP.net Core的擴充方法與工廠類別

### Install Packages(安裝)
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.AspNetCore)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.AspNetCore)
```
PM> Install-Package EzCoreKit.AspNetCore
```

### Get Started(快速上手)
```
EzCoreKit
    AspNetCore
        EntityFrameworkCore
            DbContextExtensions
                DbContext.GetOptions: 取得該DbContext初始化之Options
        Http
            HttpContextFactory
                IApplicationBuilder.UseCurrentHttpContext: 使用CurrentHttpContext
                CurrentHttpContext: 取得目前的HttpContext
        Mvc
            請至目錄內觀看ReadMe.md
        WebSockets
            請至目錄內觀看ReadMe.md
        ConfigurationExtension
            IConfiguration.ToDynamicObject: 將Configuration物件轉換為Dynamic物件
        StartupRoot
            簡單化的ASP.net Core啟動類別
```