using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="Stream"/>的擴充方法
    /// </summary>
    public static partial class StreamExtension {
        /// <summary>
        /// 串流轉換為<see cref="byte[]"/>
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns><see cref="byte[]"/>結果</returns>
        public static byte[] ToBytes(this Stream obj) {
            if (obj.CanSeek) {

                byte[] bytes = new byte[obj.Length];

                obj.Read(bytes, 0, bytes.Length);
                obj.Seek(0, SeekOrigin.Begin);

                return bytes;
            } else {
                List<byte> result = new List<byte>();
                int i;
                while ((i = obj.ReadByte()) > -1) {
                    result.Add((byte)i);
                };
                return result.ToArray();
            }

        }

        /// <summary>
        /// <see cref="byte[]"/>轉換為<see cref="Stream"/>
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns><see cref="Stream"/>結果</returns>
        public static Stream ToStream(this byte[] obj) {
            Stream stream = new MemoryStream(obj);
            return stream;
        }
    }
}
