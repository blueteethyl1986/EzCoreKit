using System;
using System.Linq;
using System.Reflection;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對類別相關功能擴充方法
    /// </summary>
    public static partial class TypeExtension {
        /// <summary>
        /// 取得指定類別繼承鏈中所有類別
        /// </summary>
        /// <param name="type">目前類別</param>
        /// <returns>類別繼承鏈所有類別</returns>
        public static Type[] GetAllBaseTypes(this Type type) {
            if (type == typeof(object)) {
                return typeof(object).BoxingToArray();
            }
            return type.GetAllBaseTypes().Union(type.BoxingToArray()).ToArray();
        }

        /// <summary>
        /// 取得指定類別繼承鏈中所有類別
        /// </summary>
        /// <param name="type">目前類別</param>
        /// <returns>類別繼承鏈所有類別</returns>
        public static TypeInfo[] GetAllBaseTypes(this TypeInfo typeInfo) {
            if (typeInfo == typeof(object).GetTypeInfo()) {
                return typeof(object).GetTypeInfo().BoxingToArray();
            }
            return typeInfo.GetAllBaseTypes().Union(typeInfo.BoxingToArray()).ToArray();
        }

        /// <summary>
        /// 確認目前類別是否為匿名類別
        /// </summary>
        /// <param name="type">目前類別</param>
        /// <returns>是否為匿名類別</returns>
        public static bool IsAnonymousType(this Type type) {
            if (type == null) return false;
            return type.Namespace == null;
        }

        /// <summary>
        /// 取得目前實例類別的預設值
        /// </summary>
        /// <typeparam name="T">實例類別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <returns>目前實例類別的預設值</returns>
        public static T GetDefault<T>(this T obj) => default(T);
    }
}
