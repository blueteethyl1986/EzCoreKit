using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest.Attributes {
    [AttributeUsage(AttributeTargets.Interface)]
    public class RestBaseUriAttribute: Attribute{
        /// <summary>
        /// 設定或取得該RestClient基礎Uri
        /// </summar>
        public string BaseUriString{get;set;}
    }
}