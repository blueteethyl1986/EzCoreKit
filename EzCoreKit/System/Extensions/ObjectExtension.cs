using EzCoreKit.System.Dynamic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EzCoreKit.System.Extensions {
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
    }
}
