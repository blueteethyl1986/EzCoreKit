using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class StreamExtension {
        /// <summary>
        /// 串流轉換為byte[]
        /// </summary>
        public static byte[] ToBytes(this Stream Obj) {
            if (Obj.CanSeek) {

                byte[] bytes = new byte[Obj.Length];

                Obj.Read(bytes, 0, bytes.Length);
                Obj.Seek(0, SeekOrigin.Begin);

                return bytes;
            } else {
                List<byte> result = new List<byte>();
                int i;
                while ((i = Obj.ReadByte()) > -1) {
                    result.Add((byte)i);
                };
                return result.ToArray();
            }

        }

        /// <summary>
        /// byte[]轉換為串流
        /// </summary>
        public static Stream ToStream(this byte[] Obj) {
            Stream stream = new MemoryStream(Obj);
            return stream;
        }
    }
}
