using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.AspNetCore.Ruleflow.Models {
    /// <summary>
    /// 存取條件
    /// </summary>
    public class AccessRule {
        public class Params {
            public HttpContext httpContext;
            public string id;
            public string key;
            public List<string> executedRules;
        }
        public static List<string> ImportNamespaces { get; set; }
            = new List<string>(new string[] {
                "System",
                "System.Linq",
                "Microsoft.AspNetCore.Http",
                "System.Collections",
                "System.Collections.Generic"
            });

        public static List<string> References { get; set; }
            = new List<string>(new string[]{
                "System",
                "System.Linq",
                "System.Collections",
                "System.Collections.Generic"
            });

        /// <summary>
        /// 規則唯一識別號
        /// </summary>

        public Guid Id { get; set; }
        
        /// <summary>
        /// 規則名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程式碼
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 是否啟用規則
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 執行規則
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="id">唯一識別號</param>
        /// <param name="key">金鑰</param>
        /// <returns>執行結果，下一個規則Guid、null(未通過規則)、Guid.Empty(規則驗證結束端點)</returns>
        public async Task<string> Run(HttpContext httpContext, string id, string key,List<string> executedRules) {
            if (!Enable) return null;//斷路
                                     // Add reference to mscorlib
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(global::System.Linq.Enumerable).GetTypeInfo().Assembly;

            var references = new[] { mscorlib, systemCore };
            var loader = new InteractiveAssemblyLoader();
            foreach (var reference in references) {
                loader.RegisterDependency(reference);
            }

            var options = ScriptOptions.Default;
            foreach (var @namespace in AccessRule.ImportNamespaces) {
                options = options.WithReferences(@namespace).WithImports(@namespace);
            }
            var script = CSharpScript.Create<string>(
                code: Code,
                globalsType: typeof(Params),
                options: options,
                assemblyLoader: loader);
            return await script.CreateDelegate()(new Params() {
                httpContext = httpContext,
                id = id,
                key = key,
                executedRules = executedRules
            });
        }
    }
}