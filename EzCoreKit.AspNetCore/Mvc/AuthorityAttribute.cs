using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 存取權限設定
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorityAttribute : Attribute {
        /// <summary>
        /// 能夠使用此操作的最低權限，必須為列舉類型
        /// </summary>
        public object Minimum { get; set; }
    }
}
