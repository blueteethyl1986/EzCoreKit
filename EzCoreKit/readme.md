EzCoreKit
=====
[![xpy MyGet Build Status](https://www.myget.org/BuildSource/Badge/xpy?identifier=9e998e97-5cd7-475e-bf52-1c1ffed913f4)](https://www.myget.org/)

Extension Methods & Factory Class for .Net Core

針對.Net Core的擴充方法與工廠類別

### Install Packages(安裝)
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.AspNetCore)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.AspNetCore)
```
PM> Install-Package EzCoreKit
```

### Get Started(快速上手)
```
EzCoreKit
    Cryptography
        HashFactory
            ToHash<T>(): 將輸入字串轉換為指定雜湊算法結果Binary
            ToHashString<T>(): 如ToHash<T>()，但轉換為十六進位字串
            備註: T: HashAlgorithm，如: MD5
    Dynamic
        ExpandoObjectFactory
            ConvertToExpando(): 將物件轉換為Dynamic的ExpandoObject            
    Extensions
        常用類型擴充方法，詳細請自行參閱xmlDoc
    File
        請至目錄內觀看ReadMe.md
    NodaTime
        InstantFactory
            ConvertToJsTime(): 將Noda實體轉換為JS時間表示
            ConvertToInstant(): 自JS時間表示轉換為Noda實體
    Reflection
        請至目錄內觀看ReadMe.md
    Threading
        TaskFactory
            LimitedTask(): 等待Action在指定的毫秒限制內完成執行，否則強制結束。
```