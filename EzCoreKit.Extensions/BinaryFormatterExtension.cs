using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="BinaryFormatter"/>的擴充方法
    /// </summary>
    public static partial class BinaryFormatterExtension {
        /// <summary>
        /// 將目標實例序列化為Binary結果
        /// </summary>
        /// <param name="obj"><see cref="BinaryFormatter"/>實例</param>
        /// <param name="graph">目標實例</param>
        /// <returns>目標實例序列化結果</returns>
        public static byte[] Serialize(this BinaryFormatter obj, object graph) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, graph);
            return ms.ToBytes();
        }


        /// <summary>
        /// 將Binary結果反序列化為目標類別實例
        /// </summary>
        /// <param name="obj"><see cref="BinaryFormatter"/>實例</param>
        /// <param name="type">目標類別</param>
        /// <param name="binary">目標實例Binary結果</param>
        /// <returns>目標實例</returns>
        public static object Deserialize(this BinaryFormatter obj, Type type, byte[] binary) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(binary);
            return Convert.ChangeType(bf.Deserialize(ms), type);
        }

        /// <summary>
        /// 將Binary結果反序列化為目標類別實例
        /// </summary>
        /// <typeparam name="T">目標類別</typeparam>
        /// <param name="obj"><see cref="BinaryFormatter"/>實例</param>
        /// <param name="binary">目標實例Binary結果</param>
        /// <returns>目標實例</returns>
        public static T Deserialize<T>(this BinaryFormatter obj, byte[] binary) {
            return (T)obj.Deserialize(typeof(T), binary);
        }
    }
}
