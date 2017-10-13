using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest.Attributes {
    /// <summary>
    /// 標註與設定Rest Client方法
    /// </summar>
    [AttributeUsage(AttributeTargets.Method)]
    public class RestMethodAttribute: Attribute{
        /// <summary>
        /// 設定或取得該REST API Uri
        /// </summar>
        public string UriString{ get; set; }
    }
}