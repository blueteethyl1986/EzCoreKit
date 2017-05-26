using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Http {
    public static class HttpContextFactory {
        public static void UseCurrentHttpContext(this IApplicationBuilder builder) {
            HttpContextFactory.app = builder;
        }


        internal static IApplicationBuilder app;
        public static HttpContext CurrentHttpContext {
            get {
                var factory = app.ApplicationServices.GetService(typeof(IHttpContextAccessor));
                return ((HttpContextAccessor)factory).HttpContext;
            }
        }
    }
}
