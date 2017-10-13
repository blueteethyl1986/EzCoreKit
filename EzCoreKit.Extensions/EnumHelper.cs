using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對陣列相關功能幫助類別
    /// </summary>
    public static partial class EnumHelper {
        /// <summary>
        /// 剖析字串至指定列舉類別值
        /// </summary>
        /// <typeparam name="T">列舉類別</typeparam>
        /// <param name="enumName">列舉值名稱</param>
        /// <param name="ignoreCase">忽略大小寫</param>
        /// <returns>列舉值</returns>
        public static T Parse<T>(string enumName, bool ignoreCase = true) where T : struct{
            return (T)Enum.Parse(typeof(T), enumName, ignoreCase);
        }

        /// <summary>
        /// 取得目標列舉值之名稱
        /// </summary>
        /// <param name="value">列舉值</param>
        /// <returns>目標列舉值名稱</returns>
        public static string GetEnumName(object value) {
            return Enum.GetName(value.GetType(), value);
        }

        /// <summary>
        /// 取得目標列舉值之名稱
        /// </summary>
        /// <typeparam name="T">列舉類別</typeparam>
        /// <param name="value">列舉值</param>
        /// <returns>目標列舉值名稱</returns>
        public static string GetEnumName<T>(this T value) where T : struct {
            return Enum.GetName(typeof(T), value);
        }

        /// <summary>
        /// 取得目標列舉值之<see cref="Attribute"/>集合
        /// </summary>
        /// <param name="attributeType">目標<see cref="Attribute"/>類別</param>
        /// <param name="value">列舉值</param>
        /// <returns>Attribute集合</returns>
        public static IEnumerable<Attribute> GetCustomAttributes(Type attributeType, object value) {
            var isEnum = value.GetType().GetTypeInfo().IsValueType;
            if (!isEnum) throw new NotSupportedException("value is not Enum");
            var typeinfo = value.GetType().GetTypeInfo();
            var fieldInfo = typeinfo.GetField(GetEnumName(value));
            return fieldInfo.GetCustomAttributes(attributeType);
        }

        /// <summary>
        /// 取得目標列舉值之<see cref="Attribute"/>
        /// </summary>
        /// <param name="attributeType">目標<see cref="Attribute"/>類別</param>
        /// <param name="value">列舉值</param>
        /// <returns>Attribute</returns>
        public static Attribute GetCustomAttribute(Type attributeType, object value) {
            var isEnum = value.GetType().GetTypeInfo().IsValueType;
            if (!isEnum) throw new NotSupportedException("value is not Enum");
            var typeinfo = value.GetType().GetTypeInfo();
            var fieldInfo = typeinfo.GetField(GetEnumName(value));
            return fieldInfo.GetCustomAttribute(attributeType);
        }

        /// <summary>
        ///  取得目標列舉值之<see cref="Attribute"/>集合
        /// </summary>
        /// <typeparam name="TAttribute">目標<see cref="Attribute"/>類別</typeparam>
        /// <param name="value">列舉值</param>
        /// <returns>Attribute集合</returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(object value)
            where TAttribute : Attribute {
            return GetCustomAttributes(typeof(TAttribute), value).Cast<TAttribute>();
        }

        /// <summary>
        /// 取得目標列舉值之<see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">目標<see cref="Attribute"/>類別</typeparam>
        /// <param name="value">列舉值</param>
        /// <returns>Attribute</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(object value)
            where TAttribute : Attribute {
            return (TAttribute)GetCustomAttribute(typeof(TAttribute), value);
        }
    }
}
