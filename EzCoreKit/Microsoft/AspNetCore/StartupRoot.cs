using EzCoreKit.Microsoft.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.Microsoft.AspNetCore {
    public class StartupRoot {
        /// <summary>
        /// 設定值
        /// </summary>
        public IConfigurationRoot Configuration { get; protected set; }

        /// <summary>
        /// 錯誤頁面與錯誤代碼對應
        /// </summary>
        public Dictionary<int, string> ErrorPages { get; private set; } = new Dictionary<int, string>();

        /// <summary>
        /// 錯誤時是否傳送狀態碼
        /// </summary>
        public static bool ExceptionHttpStatusCode { get; set; } = false;

        /// <summary>
        /// 此方法在執行階段被呼叫，使用此方法設定預設的檔案
        /// </summary>
        public void ConfigureDefaultFiles(IApplicationBuilder app) {
            //讀取設定檔中預設檔案設定
            var defaultFiles = Configuration.GetSection("defaultFiles")?.GetChildren();

            //未設定則跳脫
            if (defaultFiles == null) return;

            foreach (var defaultFileItem in defaultFiles) {
                dynamic defaultFileObj = defaultFileItem.ToDynamicObject();
                app.UseDefaultFiles(new DefaultFilesOptions() {
                    RequestPath = defaultFileObj.RequestPath,
                    DefaultFileNames = ((object[])defaultFileObj.DefaultFileNames).Select(item => (string)item).ToList()
                });
            }
        }

        /// <summary>
        /// 此方法在執行階段被呼叫，使用此方法設定MVC預設路由規則
        /// </summary>
        /// <param name="routes">路由建構器</param>
        public void ConfigureMvcRoute(IRouteBuilder routes) {
            //取得所有路由規則
            var rules = Configuration.GetSection("mvcRoutingRules")?.GetChildren();

            //未設定則跳脫
            if (rules == null) return;

            //註冊所有路由規則
            foreach (var rule in rules) {
                //取得子屬性集合
                var attributes = rule.GetChildren();

                Console.WriteLine(attributes.Where(item => item.Key == "Template").FirstOrDefault()?.Value);
                //註冊路由
                routes.MapRoute(
                    name: attributes.Where(item => item.Key == "Name").FirstOrDefault()?.Value,
                    template: attributes.Where(item => item.Key == "Template").FirstOrDefault()?.Value,
                    defaults: attributes.Where(item => item.Key == "Defaults").FirstOrDefault()?.ToDynamicObject(),
                    constraints: attributes.Where(item => item.Key == "Constraints").FirstOrDefault()?.ToDynamicObject(),
                    dataTokens: attributes.Where(item => item.Key == "DataTokens").FirstOrDefault()?.ToDynamicObject()
                );
            }
        }

        /// <summary>
        /// 此方法在執行階段被呼叫，使用此方法設定錯誤頁面的檔案
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void ConfigureErrorPages(IApplicationBuilder app, IHostingEnvironment env) {
            //取得所有錯誤頁面設定
            var pages = Configuration.GetSection("errorPages")?.GetChildren();

            //未設定則跳脫
            if (pages == null) return;

            //讀取所有錯誤頁面對應
            foreach (var page in pages) {
                //取得子屬性集合
                dynamic obj = page.ToDynamicObject();

                //存入狀態對應
                ErrorPages[int.Parse(obj.StatusCode)] = obj.FilePath;
            }

            //錯誤處理
            Action<IApplicationBuilder> ErrorHandler = (builder) => {
                builder.Run(handler => {
                    return Task.Run(() => {
                        //取得狀態碼
                        int code = handler.Response.StatusCode;

                        //檢查是否存在指定的對應
                        if (ErrorPages.ContainsKey(code)) {
                            //寫出錯誤頁面內容
                            byte[] Content = File.ReadAllBytes(env.WebRootPath + "/" + ErrorPages[handler.Response.StatusCode]);
                            handler.Response.ContentType = "text/html";
                            handler.Response.Body.WriteAsync(Content, 0, Content.Length);
                            return;

                            //以跳轉的方式到錯誤頁面
                            //handler.Response.Redirect($"{handler.Request.PathBase}/{ErrorPages[handler.Response.StatusCode]}");
                        }
                    });
                });
            };

            //狀態對應
            app.UseStatusCodePages(ErrorHandler);

            //例外對應
            app.UseExceptionHandler(ErrorHandler);
        }
    }
}
