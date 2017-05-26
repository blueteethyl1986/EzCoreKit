using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace EzCoreKit.Extensions {
    public static class TypeExtension {
        /// <summary>
        /// 取得指定的類別繼承鍊所有的基礎類別
        /// </summary>
        /// <param name="Type">指定類別</param>
        public static TypeInfo[] AllBaseTypes(this TypeInfo type) {
            if (type == typeof(object).GetTypeInfo()) return new TypeInfo[] { typeof(object).GetTypeInfo() };
            return AllBaseTypes(type.BaseType.GetTypeInfo()).Union(new TypeInfo[] { type }).ToArray();
        }

        /// <summary>
        /// 確認目前類別是否為匿名型別
        /// </summary>
        public static bool IsAnonymousType(this Type type) {
            if (type == null) return false;
            return type.Namespace == null;
        }


        public static T GetDefault<T>(this T type) => default(T);
    }
}
