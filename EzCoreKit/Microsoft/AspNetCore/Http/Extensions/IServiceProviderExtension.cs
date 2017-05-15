using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Microsoft.AspNetCore.Http.Extensions {
    public static class IServiceProviderExtension {
        public static T GetService<T>(this IServiceProvider spv){
            return (T)spv.GetService(typeof(T));
        }
    }
}
