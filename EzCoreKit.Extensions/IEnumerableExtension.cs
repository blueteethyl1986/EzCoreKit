using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對集合相關類別擴充方法
    /// </summary>
    public static partial class IEnumerableExtension {
        /// <summary>
        /// 檢查目前<see cref="IEnumerable{T}"/>實例元素數量是否為0
        /// </summary>
        /// <param name="obj">目前<see cref="IEnumerable{T}"/>實例</param>
        /// <returns>檢查元素數量是否為0</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> obj) {
            return obj.Count() == 0;
        }

        /// <summary>
        /// 檢查目前<see cref="IEnumerable{T}"/>實例元素數量是否不為0
        /// </summary>
        /// <param name="obj">目前<see cref="IEnumerable{T}"/>實例</param>
        /// <returns>檢查元素數量是否不為0</returns>
        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> obj) {
            return !obj.IsEmpty();
        }
    }
}
