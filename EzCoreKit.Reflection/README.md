# EzCoreKit.Reflection
針對System.Reflection提供擴充。

### 安裝
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.Reflection)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.Reflection)
```
PM> Install-Package EzCoreKit.Reflection
```

### 快速上手
```csharp
//get `Count` property's MemberInfo
typeof(List<int>)
    .GetMember<List<int>>(x => x.Count);

//get List<int> constructor
typeof(List<int>)
    .GetMember(x => new List<int>());

//get Clear method's MemberInfo(not return value method)
typeof(List<int>)
    .GetMember<List<int>>(x => x.Clear());

//get Sum method's MemberInfo(has return value method)
typeof(List<int>)
    .GetMember<List<int>>(x => x.Sum());

//other way
List<int> test = new List<int>();
test.GetMember(x => x.Count);
test.GetMember(x => new List<int>());
test.GetMember<List<int>>(x => x.Clear());
test.GetMember<List<int>>(x => x.Sum());
```