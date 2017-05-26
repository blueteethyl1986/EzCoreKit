using EzCoreKit.Extensions;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.NodaTime {
    public static class InstantFactory {
        /// <summary>
        /// 轉換為Js時間表示
        /// </summary>
        /// <param name="instance">時間</param>
        /// <returns>Js時間表示</returns>
        public static long ConvertToJsTime(Instant instance) {
            return instance.ToDateTimeUtc().ToJsTime();
        }

        /// <summary>
        /// 自Js時間表示轉換
        /// </summary>
        /// <param name="jsTime">Js時間表示</param>
        /// <returns>時間</returns>
        public static Instant ConvertToInstant(long jsTime) {
            return global::NodaTime.Instant.FromDateTimeUtc(DateTimeFactory.ConvertFromJsTime(jsTime));
        }
    }
}
