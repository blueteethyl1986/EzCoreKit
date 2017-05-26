using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class DateTimeExtension {
        /// <summary>
        /// 轉換為Js時間表示
        /// </summary>
        /// <param name="datetime">時間</param>
        /// <returns>Js時間表示</returns>
        public static long ToJsTime(this DateTime datetime) {
            return DateTimeFactory.ConvertToJsTime(datetime);
        }
    }
}
