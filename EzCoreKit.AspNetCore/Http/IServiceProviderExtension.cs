using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Http {
    public static class IServiceProviderExtension {
        public static T GetService<T>(this IServiceProvider spv){
            return (T)spv.GetService(typeof(T));
        }
    }
}
