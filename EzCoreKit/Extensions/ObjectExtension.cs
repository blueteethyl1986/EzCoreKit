using BinaryFormatter;
using EzCoreKit.Dynamic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using EzCoreKit.Reflection;
using System.Reflection;

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
        
        public static object ToObject(this byte[] bytes , Type type) {
            return typeof(ObjectExtension)
                .GetMethod("ToObject", BindingFlags.Static | BindingFlags.Public)
                .Invoke(new Type[] { type}, new object[] { bytes });
        }

        /// <summary>
        /// 傳回這個物件的深層副本。
        /// </summary>
        /// <param name="target"></param>
        /// <returns>深層副本</returns>
        public static object DeepClone(this object target) {
            Type TargetType = target.GetType();

            if (TargetType.GetTypeInfo().IsValueType || target is string) return target;//實質型別或字串

            object result = null;
            if (TargetType.IsArray) {//陣列型別
                Array TargetAry = (Array)target;
                result = Array.CreateInstance(TargetType.GetElementType(),  TargetAry.GetLengths());

                foreach (var index in TargetAry.GetAllIndexes()) {
                    object Value = TargetAry.GetValue(index)?.DeepClone();
                    ((Array)result).SetValue(Value, index);
                }
            } else {
                result = target.ToBytes().ToObject(target.GetType());
                foreach (var Field in TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {//設定所有欄位
                    object Value = Field.GetValue(target)?.DeepClone();//取得來源物件且取得深層副本
                    Field.SetValue(result, Value);//設定值
                }
            }
            return result;
        }
    }
}
