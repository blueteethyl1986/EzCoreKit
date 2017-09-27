using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對物件相關擴充方法
    /// </summary>
    public static partial class ObjectExtension {
        /// <summary>
        /// 將目前實例轉換為Binary結果
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns>Binary結果</returns>
        public static byte[] ToBytes(this object obj) {
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Serialize(obj);
        }

        /// <summary>
        /// 將Binary結果轉換為目標類別實例
        /// </summary>
        /// <typeparam name="T">目標類別</typeparam>
        /// <param name="bytes">Binary結果</param>
        /// <returns>目標類別實例</returns>
        public static T ToObject<T>(this byte[] bytes) {
            return (T)bytes.ToObject(typeof(T));
        }

        /// <summary>
        /// 將Binary結果轉換為目標類別實例
        /// </summary>
        /// <param name="bytes">Binary結果</param>
        /// <param name="type">目標類別</param>
        /// <returns>目標類別實例</returns>
        public static object ToObject(this byte[] bytes, Type type) {
            BinaryFormatter sf = new BinaryFormatter();
            return sf.Deserialize(type, bytes);
        }

        /// <summary>
        /// 取得目前實例深層副本
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns>目前實例深層副本</returns>
        public static object DeepClone(this object obj) {
            Type TargetType = obj.GetType();

            if (TargetType.GetTypeInfo().IsValueType || obj is string) {
                return obj;//實質型別或字串
            }

            object result = null;
            if (TargetType.IsArray) {//陣列型別
                Array TargetAry = (Array)obj;
                result = Array.CreateInstance(
                    TargetType.GetElementType(),
                    TargetAry.GetLengths()
                );

                foreach (var index in TargetAry.GetAllIndexes()) {
                    object Value = TargetAry.GetValue(index)?.DeepClone();
                    ((Array)result).SetValue(Value, index);
                }
            } else {
                result = obj.ToBytes().ToObject(obj.GetType());
                foreach (var Field in TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {//設定所有欄位
                    object Value = Field.GetValue(obj)?.DeepClone();//取得來源物件且取得深層副本
                    Field.SetValue(result, Value);//設定值
                }
            }
            return result;
        }

        /// <summary>
        /// 取得目前實例深層副本
        /// </summary>
        /// <typeparam name="T">目前實例類別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <returns>目前實例深層副本</returns>
        public static T DeepClone<T>(this T obj) {
            return (T)DeepClone(obj);
        }
    }
}
