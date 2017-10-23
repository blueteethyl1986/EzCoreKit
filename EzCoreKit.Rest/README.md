# EzCoreKit.Rest
實作AOP類型的Rest API Client。

### 安裝
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.Rest)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.Rest)
```
PM> Install-Package EzCoreKit.Linq
```

### 快速上手
```csharp
[RestBaseUri("https://jsonplaceholder.typicode.com/")]
public interface IFakeAPI {
    [RestMethod(//Default Method is Get
        Uri = "posts",
        ResponseFormat = DataFormat.Json)]
    Task<Post[]> GetPosts([RestQueryParameter]int? userId = null);

    [RestMethod(
        Uri = "http://opendata.cwb.gov.tw/govdownload?dataid=E-A0015-001R&authorizationkey=rdec-key-123-45678-011121314",
        ResponseFormat = DataFormat.Xml,
        Path = "//location/text()")]
    Task<string> GetEarthquakesLocation();
}

public class RestClientTest {
    public async void CreateInstance() {
        RestClientBuilder<IFakeAPI> builder = new RestClientBuilder<IFakeAPI>();
        var client = builder.Build();

        var allPost = await client.GetPosts();
        var user2Post = await client.GetPosts(2);
        var post1Comments = await client.GetComments(1);
    }
}
```