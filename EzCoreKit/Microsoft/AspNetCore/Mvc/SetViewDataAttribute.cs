using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Microsoft.AspNetCore.Mvc {
    /// <summary>
    /// 執行Action前先行設定ViewData
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SetViewDataAttribute : Attribute {
        /// <summary>
        /// 鍵值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
