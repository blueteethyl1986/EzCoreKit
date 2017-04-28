using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.System {
    public class DateTimeFactory {
        /// <summary>
        /// 轉換為Js時間表示
        /// </summary>
        /// <param name="datetime">時間</param>
        /// <returns>Js時間表示</returns>
        public static long ConvertToJsTime(DateTime datetime) {
            return (long)(datetime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 自Js時間表示轉換
        /// </summary>
        /// <param name="jsTime">Js時間表示</param>
        /// <returns>時間</returns>
        public static DateTime ConvertFromJsTime(long jsTime) {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(jsTime);
        }
    }
}
