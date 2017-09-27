using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="DateTime"/>相關功能擴充方法
    /// </summary>
    public static partial class DateTimeExtension {
        /// <summary>
        /// 將目前<see cref="DateTime"/>實例轉換為Unix Timestamp
        /// </summary>
        /// <param name="datetime">目前實例</param>
        /// <returns><see cref="DateTime"/>實例結果</returns>
        public static long ToUnixTimestamp(this DateTime datetime) {
            return DateTimeConvert.ToUnixTimestamp(datetime);
        }
    }
}
