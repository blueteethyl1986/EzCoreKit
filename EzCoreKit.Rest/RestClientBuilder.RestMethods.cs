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
using EzCoreKit.Rest.Attributes.Paramters;
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
            if(methodSetting.Uri.IsMatch("^https?://.*")){
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
                try{
                    task.SetResult(ConvertType(caller, response));
                }catch(Exception e){
                    task.SetException(e);
                }
            });
            return await task.Task;
        }

        private static void SettingRestRequestParameters(RestRequest request, object instance, MethodBase caller, object[] parameters) {
            var instanceType = instance.GetType();

            #region Header設定
            foreach (var interfaceHeader in instanceType.GetCustomAttributes<RestHeaderParameterAttribute>()) {
                request.AddHeader(interfaceHeader.Name, interfaceHeader.Value);
            }
            foreach (var methodHeader in caller.GetCustomAttributes<RestHeaderParameterAttribute>()) {
                request.AddHeader(methodHeader.Name, methodHeader.Value);
            }
            foreach (var parameterHeader in caller.GetParameters().Select(x => new {
                paramter = x,
                attribute = x.GetCustomAttribute<RestHeaderParameterAttribute>()
            })) {
                var name = parameterHeader.attribute.Name;
                if (parameterHeader.attribute.Name == null) {
                    name = parameterHeader.paramter.Name;
                }
                var value = parameterHeader.attribute.Value;
                if (parameterHeader.attribute.Value == null) {
                    value = parameters[parameterHeader.paramter.Position] as string;
                }
                if(value != null)request.AddHeader(name, value);
            }
            #endregion

            #region Cookie設定
            foreach (var interfaceCookie in instanceType.GetCustomAttributes<RestCookieParameterAttribute>()) {
                request.AddCookie(interfaceCookie.Name, interfaceCookie.Value);
            }
            foreach (var methodCookie in caller.GetCustomAttributes<RestCookieParameterAttribute>()) {
                request.AddCookie(methodCookie.Name, methodCookie.Value);
            }
            foreach (var parameterCookie in caller.GetParameters().Select(x => new {
                paramter = x,
                attribute = x.GetCustomAttribute<RestCookieParameterAttribute>()
            })) {
                var name = parameterCookie.attribute.Name;
                if (parameterCookie.attribute.Name == null) {
                    name = parameterCookie.paramter.Name;
                }
                var value = parameterCookie.attribute.Value;
                if (parameterCookie.attribute.Value == null) {
                    value = parameters[parameterCookie.paramter.Position] as string;
                }
                if(value != null)request.AddCookie(name, value);
            }
            #endregion

            #region Parameter設定
            foreach (var interfaceParameter in instanceType.GetCustomAttributes<RestFormParameterAttribute>()) {
                request.AddCookie(interfaceParameter.Name, interfaceParameter.Value);
            }
            foreach (var methodParameter in caller.GetCustomAttributes<RestFormParameterAttribute>()) {
                request.AddCookie(methodParameter.Name, methodParameter.Value);
            }
            foreach (var parameterParameter in caller.GetParameters().Select(x => new {
                paramter = x,
                attribute = x.GetCustomAttribute<RestFormParameterAttribute>()
            })) {
                var name = parameterParameter.attribute.Name;
                if (parameterParameter.attribute.Name == null) {
                    name = parameterParameter.paramter.Name;
                }
                var value = parameterParameter.attribute.Value;
                if (parameterParameter.attribute.Value == null) {
                    value = parameters[parameterParameter.paramter.Position] as string;
                }
                if(value != null)request.AddParameter(name, value);
            }
            #endregion

            #region QueryParameter設定
            foreach (var interfaceQueryParameter in instanceType.GetCustomAttributes<RestQueryParameterAttribute>()) {
                request.AddCookie(interfaceQueryParameter.Name, interfaceQueryParameter.Value);
            }
            foreach (var methodQueryParameter in caller.GetCustomAttributes<RestQueryParameterAttribute>()) {
                request.AddCookie(methodQueryParameter.Name, methodQueryParameter.Value);
            }
            foreach (var parameterQueryParameter in caller.GetParameters().Select(x => new {
                paramter = x,
                attribute = x.GetCustomAttribute<RestQueryParameterAttribute>()
            })) {
                var name = parameterQueryParameter.attribute.Name;
                if (parameterQueryParameter.attribute.Name == null) {
                    name = parameterQueryParameter.paramter.Name;
                }
                var value = parameterQueryParameter.attribute.Value;
                if (parameterQueryParameter.attribute.Value == null) {
                    value = parameters[parameterQueryParameter.paramter.Position] as string;
                }
                if(value != null)request.AddQueryParameter(name, value);
            }
            #endregion

            #region UrlParameter設定
            foreach (var interfaceUrlParameter in instanceType.GetCustomAttributes<RestUrlParameterAttribute>()) {
                request.AddCookie(interfaceUrlParameter.Name, interfaceUrlParameter.Value);
            }
            foreach (var methodUrlParameter in caller.GetCustomAttributes<RestUrlParameterAttribute>()) {
                request.AddCookie(methodUrlParameter.Name, methodUrlParameter.Value);
            }
            foreach (var parameterUrlParameter in caller.GetParameters().Select(x => new {
                paramter = x,
                attribute = x.GetCustomAttribute<RestUrlParameterAttribute>()
            })) {
                var name = parameterUrlParameter.attribute.Name;
                if (parameterUrlParameter.attribute.Name == null) {
                    name = parameterUrlParameter.paramter.Name;
                }
                var value = parameterUrlParameter.attribute.Value;
                if (parameterUrlParameter.attribute.Value == null) {
                    value = parameters[parameterUrlParameter.paramter.Position] as string;
                }
                if(value != null)request.AddUrlSegment(name, value);
            }
            #endregion
        }
    }
}