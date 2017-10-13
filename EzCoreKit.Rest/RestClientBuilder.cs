using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;
using RestSharp;

namespace EzCoreKit.Rest {
    /// <summary>
    /// 為RESTful Web API Client建構器
    /// </summary>
    public partial class RestClientBuilder<T> {
        ///目前Builder的uri
        private string baseUri { get; set; }
        private string auth_username { get; set; }
        private string auth_password { get; set; }
        private DataFormat requestFormat { get; set; }

        /// <summary>
        /// 建立並初始化針對<see cref="T"/>類別的RESTful Web API Client建構器
        /// </summary>
        public RestClientBuilder() {
            if (!typeof(T).IsInterface) {//必須是interface
                throw new ArgumentException($"{nameof(T)}應為interface");
            }

            //自Attribute中取得BaseUri預設值
            RestBaseUriAttribute baseUriSetting = typeof(T).GetCustomAttribute<RestBaseUriAttribute>();
            if (baseUri != null) {
                baseUri = baseUriSetting.BaseUri;
            }
        }

        /// <summary>
        /// 設定Request格式
        /// </summary>
        /// <param name="format">格式</param>
        /// <returns>為RESTful Web API Client建構器</returns>
        public RestClientBuilder<T> SetRequestFormat(DataFormat format) {
            requestFormat = format;
            return this;
        }

        /// <summary>
        /// 設定HTTP基本驗證
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns>為RESTful Web API Client建構器</returns>
        public RestClientBuilder<T> SetBasicAuthenticator(string username, string password) {
            auth_username = username;
            auth_password = password;
            return this;
        }

        /// <summary>
        /// 設定基底Uri
        /// </summary>
        /// <param name="uriString">Uri實例</param>
        /// <returns>為RESTful Web API Client建構器</returns>
        public RestClientBuilder<T> SetBaseUri(Uri uri) {
            baseUri = uri.OriginalString;
            return this;
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
            return (T)Activator.CreateInstance(ImplementInterface());
        }
    }
}