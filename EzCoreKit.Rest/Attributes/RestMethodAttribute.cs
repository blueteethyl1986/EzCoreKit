using RestSharp;
using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest.Attributes {
    /// <summary>
    /// 標註與設定Rest Client方法
    /// </summar>
    [AttributeUsage(AttributeTargets.Method)]
    public class RestMethodAttribute : Attribute {
        /// <summary>
        /// 設定或取得該REST API Uri
        /// </summar>
        public string Uri { get; set; }

        /// <summary>
        /// HTTP請求方法
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// REST API回傳結果格式
        /// </summary>
        public DataFormat ResponseFormat { get; set; }
    }
}