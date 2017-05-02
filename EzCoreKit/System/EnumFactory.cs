using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.System.Extensions {
    public static class EnumFactory {
        /// <summary>
        /// 取得指定列舉值之名稱
        /// </summary>
        /// <param name="type">列舉類型</param>
        /// <param name="value">值</param>
        /// <returns>名稱</returns>
        public static string GetEnumName(Type type, object value) {
            return Enum.GetName(type, value);
        }

        /// <summary>
        /// 取得指定列舉值名稱
        /// </summary>
        /// <typeparam name="T">列舉類型</typeparam>
        /// <param name="value">值</param>
        /// <returns>名稱</returns>
        public static string GetEnumName<T>(T value) where T : struct {
            return GetEnumName(typeof(T), value);
        }

        /// <summary>
        /// 取得自訂列舉欄位之<see cref="Attribute"/>集合
        /// </summary>
        /// <param name="type">列舉類型</param>
        /// <param name="value">值</param>
        /// <returns>Attribute集合</returns>
        public static IEnumerable<Attribute> GetCustomAttributes(Type type, object value) {
            var isEnum = value.GetType().GetTypeInfo().IsValueType;
            if (!isEnum) throw new NotSupportedException("value is not Enum");
            var typeinfo = value.GetType().GetTypeInfo();
            var fieldInfo = typeinfo.GetField(GetEnumName(value.GetType(),value));
            return fieldInfo.GetCustomAttributes(type);
        }

        /// <summary>
        /// 取得自訂列舉欄位之<see cref="Attribute"/>
        /// </summary>
        /// <param name="type">列舉類型</param>
        /// <param name="value">值</param>
        /// <returns>Attribute</returns>
        public static Attribute GetCustomAttribute(Type type, object value) {
            var isEnum = value.GetType().GetTypeInfo().IsValueType;
            if (!isEnum) throw new NotSupportedException("value is not Enum");
            var typeinfo = value.GetType().GetTypeInfo();
            var fieldInfo = typeinfo.GetField(GetEnumName(value.GetType(), value));
            return fieldInfo.GetCustomAttribute(type);
        }

        /// <summary>
        /// 取得自訂列舉欄位之<see cref="Attribute"/>集合
        /// </summary>
        /// <typeparam name="TAttribute">列舉類型</typeparam>
        /// <param name="value">值</param>
        /// <returns>Attribute集合</returns>
        public static IEnumerable<Attribute> GetCustomAttributes<TAttribute>(object value) where TAttribute : Attribute {
            return GetCustomAttributes(typeof(TAttribute), value);
        }

        /// <summary>
        /// 取得自訂列舉欄位之<see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">列舉類型</typeparam>
        /// <param name="value">值</param>
        /// <returns>Attribute</returns>
        public static Attribute GetCustomAttribute<TAttribute>(object value) where TAttribute : Attribute {
            return GetCustomAttribute(typeof(TAttribute), value);
        }
    }
}
