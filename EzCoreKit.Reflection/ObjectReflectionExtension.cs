using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Reflection {
    /// <summary>
    /// 針對物件反射相關擴充方法
    /// </summary>
    public static class ObjectReflectionExtension {
        /// <summary>
        /// 取得目前實例指定私有欄位值
        /// </summary>
        /// <typeparam name="T">值類別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns>欄位值</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string fieldName) {
            TypeInfo temp = obj.GetType().GetTypeInfo();
            while (temp != typeof(object).GetTypeInfo()) {
                var fieldInfo = obj.GetType().GetTypeInfo().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo == null) continue;
                return (T)fieldInfo.GetValue(obj);
            }
            return default(T);
        }
    }
}
