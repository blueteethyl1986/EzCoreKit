using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.System.Dynamic {
    public static class ExpandoObjectFactory {
        /// <summary>
        /// 將物件轉換為動態物件，僅欄位與屬性
        /// </summary>
        /// <param name="obj">目標物件</param>
        /// <param name="publicOnly">僅為public屬性與欄位</param>
        /// <returns>動態物件</returns>
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
