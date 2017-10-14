using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest.Attributes {
    /// <summary>
    /// 標註Rest Client之Base Uri
    /// </summar>
    [AttributeUsage(
        AttributeTargets.Interface)]
    public class RestBaseUriAttribute : Attribute {
        /// <summary>
        /// 設定或取得該Rest Client基礎Uri
        /// </summar>
        public string BaseUri { get; set; }

        public RestBaseUriAttribute(string baseUri) {
            BaseUri = baseUri;
        }
    }
}