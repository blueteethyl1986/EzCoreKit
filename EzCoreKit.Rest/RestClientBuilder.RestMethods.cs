using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;
using System.Threading.Tasks;

namespace EzCoreKit.Rest {
    //這個部分類別用以接受interface實例產生的方法引動，用以執行REST Request與返回結果
    public partial class RestClientBuilder<T> {
        /// <summary>
        /// 同步REST方法進入點
        /// </summary>
        /// <param name="instance">呼叫REST方法之實例</param>
        /// <param name="caller">呼叫來源方法</param>
        /// <param name="parameters">呼叫來源參數陣列</param>
        /// <returns>REST方法執行結果</returns>
        public static object RestRequestProcess(object instance, MethodBase caller, object[] parameters) {
            return RestRequestProcessAsync(instance, caller, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 非同步REST方法進入點
        /// </summary>
        /// <param name="instance">呼叫REST方法之實例</param>
        /// <param name="caller">呼叫來源方法</param>
        /// <param name="parameters">呼叫來源參數陣列</param>
        /// <returns>REST方法執行結果</returns>
        public static async Task<object> RestRequestProcessAsync(object instance, MethodBase caller, object[] parameters) {
            throw new NotImplementedException();
        }
    }
}