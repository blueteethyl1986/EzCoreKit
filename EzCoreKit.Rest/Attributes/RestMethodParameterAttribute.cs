using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest.Attributes {
    /// <summary>
    /// 標註與設定Rest Client方法參數
    /// </summar>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RestMethodParameterAttribute : Attribute{
        /// <summary>
        /// 參數名稱
        /// </summary>
        public string Name { get; set; }
    }
}