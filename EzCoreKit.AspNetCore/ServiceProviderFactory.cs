using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace EzCoreKit.AspNetCore {
    /// <summary>
    /// 提供於執行階段取得.NET Core DI服務提供者
    /// </summary>
    public static class ServiceProviderFactory {
        /// <summary>
        /// DI服務提供者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 使用本工廠類別於執行階段取得.NET Core DI服務提供者
        /// </summary>
        /// <param name="builder">應用程式建構器</param>
        public static void UseServiceProviderFactory(this IApplicationBuilder builder) {
            ServiceProvider = builder.ApplicationServices;
        }
    }
}
