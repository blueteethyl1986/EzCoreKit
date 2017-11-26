using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="HttpClient"/>相關功能擴充方法
    /// </summary>
    public static class HttpClientExtension {
        /// <summary>
        /// 將 GET 要求傳送至指定的 URI，並透過非同步作業，以Json形式傳回回應內容
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri">傳送要求的目標 URI</param>
        /// <returns>操作結果</returns>
        public static async Task<JToken> GetJsonAsync(this HttpClient client, string requestUri) {
            return JToken.Parse(await client.GetStringAsync(requestUri));
        }

        /// <summary>
        /// 將 GET 要求傳送至指定的 URI，並透過非同步作業，以Json形式傳回回應內容
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri">傳送要求的目標 URI</param>
        /// <returns>操作結果</returns>
        public static async Task<JToken> GetJsonAsync(this HttpClient client, Uri requestUri) {
            return JToken.Parse(await client.GetStringAsync(requestUri));
        }
    }
}
