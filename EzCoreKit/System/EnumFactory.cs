using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.System.Extensions {
    public static class EnumFactory {
        public static TAttribute GetCustomAttribute<TAttribute>(object value) where TAttribute:Attribute {
            //var isEnum = value.GetType().GetTypeInfo().IsValueType;
            var typeinfo = value.GetType().GetTypeInfo();
            var fieldInfo = typeinfo.GetField(Enum.GetName(typeof(TAttribute), value));
            return (TAttribute)fieldInfo.GetCustomAttribute<TAttribute>();
        }
    }
}
