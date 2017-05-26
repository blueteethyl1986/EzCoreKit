using BinaryFormatter;
using EzCoreKit.Dynamic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class ObjectExtension {
        /// <summary>
        /// 將物件轉換為動態物件，僅欄位與屬性
        /// </summary>
        /// <param name="obj">目標物件</param>
        /// <param name="publicOnly">僅為public屬性與欄位</param>
        /// <returns>動態物件</returns>
        public static ExpandoObject ToExpando(this object obj, bool publicOnly = true) {
            return ExpandoObjectFactory.ConvertToExpando(obj, publicOnly);
        }

        /// <summary>
        /// 自物件轉換為byte[]
        /// </summary>
        public static byte[] ToBytes(this object obj) {
            BinaryConverter sf = new BinaryConverter();            
            return sf.Serialize(obj);
        }

        /// <summary>
        /// 自byte[]還原為目標型別參數
        /// </summary>
        /// <param name="bytes">資料來源</param>
        public static T ToObject<T>(this byte[] bytes) {
            BinaryConverter sf = new BinaryConverter();
            return sf.Deserialize<T>(bytes);
        }
    }
}
