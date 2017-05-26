using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class RandomExtension {
        /// <summary>
        /// 隨機取指定數值內的double值
        /// </summary>
        public static double NextDouble(this Random rand, double min, double max) {
            return rand.NextDouble(max - min) + min;
        }

        /// <summary>
        /// 隨機取得0到指定數值內的double值
        /// </summary>
        public static double NextDouble(this Random rand, double max) {
            return rand.NextDouble() * max;
        }

        /// <summary>
        /// 隨機取得bool值
        /// </summary>
        public static bool NextBool(this Random rand) {
            return rand.NextDouble() > 0.5;
        }

        /// <summary>
        /// 隨機自列舉型別中取得值
        /// </summary>
        public static T NextEnum<T>(this Random rand) {
            return (T)NextEnum(rand, typeof(T));
        }
        /// <summary>
        /// 隨機自列舉型別中取得值
        /// </summary>
        /// <param name="type">目標型別</param>
        public static object NextEnum(this Random rand, Type type) {
            Array values = type.GetTypeInfo().GetEnumValues();
            int index = rand.Next(values.Length);
            return values.GetValue(index);
        }

        /// <summary>
        /// 隨機自字串陣列中取得值
        /// </summary>
        /// <param name="data">目標字串</param>
        public static string NextString(this Random rand, params string[] data) {
            int index = rand.Next(data.Length);
            return data[index];
        }
    }
}
