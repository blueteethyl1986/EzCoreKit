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
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace EzCoreKit.Rest {
    //這個部分類別用以接受REST返回結果與反序列化結果至指定類型
    public partial class RestClientBuilder<T> {
        private static object ConvertType(MethodBase caller, IRestResponse response) {
            var returnType = ((MethodInfo)caller).ReturnType;
            if (returnType == typeof(void)) return null;

            if (returnType.BaseType == typeof(Task)) {
                returnType = returnType.GenericTypeArguments.First();
            }

            var methodSetting = caller.GetCustomAttribute<RestMethodAttribute>() ?? new RestMethodAttribute();
            switch (methodSetting.ResponseFormat) {
                case DataFormat.Json:
                    return ConvertJson(returnType, response.Content, methodSetting.Path);
                case DataFormat.Xml:
                    return ConvertXml(returnType, response.Content, methodSetting.Path);
                default:
                    throw new NotSupportedException("RestMethodAttribute.ResponseFormat指定目標不支援");
            }
        }

        private static object ConvertJson(Type returnType, string content, string path) {
            var json = JToken.Parse(content);

            if (path != null &&
                returnType == typeof(JToken)) {
                return json.SelectToken(path);
            } else if (path != null &&
                       returnType == typeof(JArray)) {
                return new JArray(json.SelectTokens(path).ToArray());
            } else if (path != null &&
                       returnType == typeof(JObject)) {
                return (JObject)json.SelectToken(path);
            } else if (path != null &&
                 returnType.IsArray) {
                var result = json.SelectTokens(path);
                var converted = result.Select(x => x.ToObject(returnType.GetElementType())).ToArray();
                var resultAry = Array.CreateInstance(returnType.GetElementType(), converted.Length);

                for (int i = 0; i < resultAry.Length; i++) {
                    resultAry.SetValue(converted[i], i);
                }

                return resultAry;
            } else if (path != null) {
                return Convert.ChangeType(json.SelectToken(path).ToObject(returnType), returnType);
            }

            return Convert.ChangeType(json.ToObject(returnType), returnType);
        }

        private static object ConvertXml(Type returnType, string content, string path) {
            object deserialize(Type type, XmlNode xmlobj) {
                if (xmlobj.NodeType == XmlNodeType.Text && (type.GetTypeInfo().IsPrimitive || type == typeof(string))) {
                    return Convert.ChangeType(xmlobj.Value, type);
                }


                XmlSerializer ser = new XmlSerializer(type);
                XmlNodeReader reader = new XmlNodeReader(xmlobj);
                object obj = ser.Deserialize(reader);

                return obj;
            }

            XmlDocument stripDocumentNamespace(XmlDocument oldDom) {
                // Remove all xmlns:* instances from the passed XmlDocument
                // to simplify our xpath expressions.
                XmlDocument newDom = new XmlDocument();
                var tempXML = System.Text.RegularExpressions.Regex.Replace(
                        oldDom.OuterXml,
                        @"(xmlns:?[^=]*=[""][^""]*[""])",
                        "",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);

                tempXML = System.Text.RegularExpressions.Regex.Replace(
                        tempXML,
                        "<[a-zA-Z][^<>:]+:",
                        "<",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);

                tempXML = System.Text.RegularExpressions.Regex.Replace(
                        tempXML,
                        "</[a-zA-Z][^<>:]+:",
                        "</",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);

                newDom.LoadXml(tempXML);
                return newDom;
            }

            XmlNode xml = new XmlDocument();
            ((XmlDocument)xml).LoadXml(content);
            xml = stripDocumentNamespace((XmlDocument)xml);
            xml = ((XmlDocument)xml).DocumentElement;

            if (path != null &&
                returnType == typeof(XmlNode)) {
                return xml.SelectSingleNode(path);
            } else if (path != null &&
                       returnType == typeof(XmlNodeList)) {
                return xml.SelectNodes(path);
            } else if (path != null &&
                 returnType.IsArray) {
                var result = xml.SelectNodes(path);
                var resultAry = Array.CreateInstance(returnType.GetElementType(), result.Count);

                for (int i = 0; i < result.Count; i++) {
                    resultAry.SetValue(deserialize(returnType.GetElementType(), result[i]), i);
                }

                return resultAry;
            } else if (path != null) {
                return deserialize(returnType, xml.SelectSingleNode(path));
            }

            return deserialize(returnType, xml);
        }
    }
}