using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Reflection {
    public static class ObjectReflectionExtensions {
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
