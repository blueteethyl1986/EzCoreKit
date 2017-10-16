using System;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="DateTime"/>相關功能轉換方法
    /// </summary>
    public static class DateTimeConvert {
        /// <summary>
        /// 將目標<see cref="DateTime"/>實例轉換為Unix Timestamp
        /// </summary>
        /// <param name="datetime">目標實例</param>
        /// <returns>目標實例之Unix Timestamp值</returns>
        public static long ToUnixTimestamp(DateTime datetime) {
            return (long)(datetime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                    .TotalMilliseconds / 1000;
        }

        /// <summary>
        /// 將Unix Timestamp轉換為<see cref="DateTime"/>實例
        /// </summary>
        /// <param name="unixTime">Unix Timestamp</param>
        /// <returns><see cref="DateTime"/>實例結果</returns>
        public static DateTime FromUnixTimestamp(long unixTime) {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(unixTime * 1000);
        }
    }
}
