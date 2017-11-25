using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EzCoreKit.AspNetCore {
    /// <summary>
    /// 提供執行階段取得目前HttpContext物件
    /// </summary>
    public static class HttpContextFactory {
        /// <summary>
        /// 目前HttpContext物件
        /// </summary>
        public static HttpContext CurrentHttpContext {
            get {
                var factory = ServiceProviderFactory.ServiceProvider.GetService(typeof(IHttpContextAccessor));
                return ((HttpContextAccessor)factory).HttpContext;
            }
        }

        /// <summary>
        /// 使用本工廠類別於執行階段取得目前HttpContext物件
        /// </summary>
        /// <param name="builder">應用程式建構器</param>
        public static void UseCurrentHttpContext(this IApplicationBuilder builder) {
            builder.UseServiceProviderFactory();
        }
    }
}
