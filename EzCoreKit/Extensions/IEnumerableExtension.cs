using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class IEnumerableExtension {
        /// <summary>
        /// 檢查元素數量是否為0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>檢查元素數量是否為0</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> obj) {
            return obj.Count() == 0;
        }

        /// <summary>
        /// 檢查元素數量是否不為0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>檢查元素數量是否不為0</returns>
        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> obj) {
            return !obj.IsEmpty();
        }
    }
}
