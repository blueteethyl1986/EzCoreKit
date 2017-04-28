using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc {
    /// <summary>
    /// 執行Action前先行設定TempData
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SetTempDataAttribute : Attribute {
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
