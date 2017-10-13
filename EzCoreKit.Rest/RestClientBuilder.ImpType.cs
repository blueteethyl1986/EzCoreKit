using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;

namespace EzCoreKit.Rest {
    //這個部分類別用以實作interface類型，並串接至RestClientBuilder.RestMethods
    public partial class RestClientBuilder<T> {
        private Type ImplementInterface() => ImplementInterface(typeof(T));
        private Type ImplementInterface(Type interfaceType){
            throw new NotImplementedException();
        }
    }
}