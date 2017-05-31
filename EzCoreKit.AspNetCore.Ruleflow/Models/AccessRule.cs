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
        public static List<string> ReferencesAndImports { get; set; }
            = new List<string>(new string[] {
                "System",
                "System.Linq",
                "Microsoft.AspNetCore.Http",
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

            var options = ScriptOptions.Default
                .AddReferences(ReferencesAndImports)
                .AddImports(ReferencesAndImports);
            var script = CSharpScript.Create<string>(
                code: Code,
                globalsType: typeof(Params),
                options: options);
            return await script.CreateDelegate()(new Params() {
                httpContext = httpContext,
                id = id,
                key = key,
                executedRules = executedRules
            });
        }
    }
}