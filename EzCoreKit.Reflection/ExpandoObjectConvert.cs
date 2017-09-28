using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace EzCoreKit.Reflection {
    /// <summary>
    /// ExpandoObject轉換器
    /// </summary>
    public static class ExpandoObjectConvert {
        /// <summary>
        /// 將目標實例轉換為<see cref="ExpandoObject"/>實例
        /// </summary>
        /// <param name="obj">目標實例</param>
        /// <param name="publicOnly">是否只轉換Public屬性</param>
        /// <returns><see cref="ExpandoObject"/>實例</returns>
        public static ExpandoObject ConvertToExpando(object obj, bool publicOnly = true) {
            ExpandoObject result = new ExpandoObject();
            IDictionary<string, object> dict = result;
            BindingFlags flag =
                BindingFlags.Instance |
                BindingFlags.Public;
            if (!publicOnly) flag |= BindingFlags.NonPublic;
            var allMembers = obj.GetType().GetMembers(flag);

            foreach (var member in allMembers) {
                if (member is FieldInfo) {
                    dict.Add(member.Name, ((FieldInfo)member).GetValue(obj));
                } else if (member is PropertyInfo &&
                    ((PropertyInfo)member).GetIndexParameters().Length == 0) {
                    dict.Add(member.Name, ((PropertyInfo)member).GetValue(obj));
                }
            }
            return result;
        }
    }
}
