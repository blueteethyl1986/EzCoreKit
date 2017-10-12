using System;
using System.Collections.Generic;

namespace EzCoreKit.Rest {
    /// <summary>
    /// 為RESTful Web API Client建構器
    /// </summary>
    public class RestClientBuilder<T> {
        private string baseUri { get; set; }

        public RestClientBuilder() {
            if (!typeof(T).IsInterface) {//必須是interface
                throw new ArgumentException($"{nameof(T)}應為interface");
            }
        }

        /// <summary>
        /// 設定基底Uri
        /// </summary>
        /// <param name="uriString">Uri實例</param>
        /// <returns>為RESTful Web API Client建構器</returns>
        public RestClientBuilder<T> SetBaseUri(Uri uri) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 設定基底Uri
        /// </summary>
        /// <param name="uriString">Uri字串</param>
        /// <returns>為RESTful Web API Client建構器</returns>
        public RestClientBuilder<T> SetBaseUri(string uriString) {
            return SetBaseUri(new Uri(uriString));
        }

        /// <summary>
        /// 使用目前建構器的設定產生RESTful Web API Client實例
        /// </summary>
        /// <returns>RESTful Web API Client實例</returns>
        public T Build() {
            throw new NotImplementedException();
        }
    }
}