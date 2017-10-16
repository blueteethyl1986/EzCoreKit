using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;
using System.Threading.Tasks;
using EzCoreKit.Reflection;
using EzCoreKit.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
using EzCoreKit.Rest.Attributes.Parameters;
using Newtonsoft.Json.Linq;

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
            var interfaceType = caller.DeclaringType.GetInterfaces().First();
            caller = interfaceType.GetMethod(caller.Name, BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, caller.GetParameters().Select(x => x.ParameterType).ToArray(), null);

            var client = new RestClient();
            // 取得目標REST BASE API網址
            var callUri = instance.GetPrivateFieldValue<string>("baseUri");
            var auth_username = instance.GetPrivateFieldValue<string>("auth_username");
            var auth_password = instance.GetPrivateFieldValue<string>("auth_password");

            if (callUri == null) throw new InvalidOperationException("必須設定BaseUri");

            client.BaseUrl = new Uri(callUri);
            if (auth_username != null || auth_password != null) {
                client.Authenticator = new HttpBasicAuthenticator(auth_username, auth_password);
            }

            var methodSetting = caller.GetCustomAttribute<RestMethodAttribute>() ?? new RestMethodAttribute();

            var request = new RestRequest();
            if (methodSetting.Uri.IsMatch("^https?://.*")) {
                client.BaseUrl = new Uri(methodSetting.Uri);
                request.Resource = "";
            } else {
                request.Resource = methodSetting.Uri ?? "";
            }
            request.Method = methodSetting.Method;
            request.RequestFormat = instance.GetPrivateFieldValue<DataFormat>("requestFormat");

            SettingRestRequestParameters(request, instance, caller, parameters);

            var task = new TaskCompletionSource<object>();
            var t = client.ExecuteAsync(request, response => {
                try {
                    task.SetResult(ConvertType(caller, response));
                } catch (Exception e) {
                    task.SetException(e);
                }
            });
            return await task.Task;
        }

        private static void SettingRestRequestParameters(RestRequest request, object instance, MethodBase caller, object[] parameters) {
            var instanceType = instance.GetType();

            void setRequestVars(Type parameterType) {
                foreach (RestParameterAttribute parameter in instanceType.GetCustomAttributes(parameterType)) {
                    request.AddCookie(parameter.Name, parameter.Value);
                }
                foreach (RestParameterAttribute parameter in caller.GetCustomAttributes(parameterType)) {
                    request.AddCookie(parameter.Name, parameter.Value);
                }
                foreach (var parameter in caller.GetParameters().Select(x => new {
                    paramter = x,
                    attribute = x.GetCustomAttribute(parameterType) as RestParameterAttribute
                })) {
                    var name = parameter.attribute?.Name;
                    if (name == null) {
                        name = parameter.paramter.Name;
                    }
                    var value = parameter.attribute?.Value;
                    if (value == null) {
                        value = parameters[parameter.paramter.Position]?.ToString();
                    }
                    if (value != null) request.AddUrlSegment(name, value);
                }
            }

            foreach (var type in TypeHelper.GetNamespaceTypes(typeof(RestParameterAttribute).Namespace)
                .Where(x => x != typeof(RestParameterAttribute))) {
                setRequestVars(type);
            }
        }
    }
}