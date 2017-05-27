EzCoreKit.Reflection
=====
[![xpy MyGet Build Status](https://www.myget.org/BuildSource/Badge/xpy?identifier=9e998e97-5cd7-475e-bf52-1c1ffed913f4)](https://www.myget.org/)

Simple way to use Expression to Reflection. This project use Lambda Expression simplify GetMember method.

使用Lambda Expression提供更簡單的方法來實作反射，更簡單的取得物件成員

### Get Started(快速上手)
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