using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;

namespace EzCoreKit.Rest {
    //這個部分類別用以接受interface實例產生的方法引動，用以執行REST Request與返回結果
    public partial class RestClientBuilder<T> {
        public static object RestRequestProcess(object instance, MethodBase caller, object[] parameters){
            throw new NotImplementedException();
        }
    }
}