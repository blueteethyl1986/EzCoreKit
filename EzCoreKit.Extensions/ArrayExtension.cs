using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對陣列相關功能擴充方法
    /// </summary>
    public static partial class ArrayExtension {
        /// <summary>
        /// 將目前實例包裝為目前實體類別陣列
        /// </summary>
        /// <typeparam name="T">目前實例類別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <returns>目前實體類別陣列包裝結果</returns>
        public static T[] BoxingToArray<T>(this T obj) {
            return new T[] { obj };
        }

        /// <summary>
        /// 取得代表<see cref="System.Array"/>所有維度之元素數目的 32 位元整數
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns>所有維度之元素數目的 32 位元整數</returns>
        public static int[] GetLengths(this Array obj) {
            return Enumerable.Range(0, obj.Rank)
                .Select(x => obj.GetLength(x)).ToArray();
        }

        /// <summary>
        /// 取得代表<see cref="System.Array"/>所有元素的索引
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns>32 位元的整數陣列的物件清單，代表所有元素的索引</returns>
        public static List<List<int>> GetAllIndexes(this Array obj) {
            List<int> Indexes = obj.GetLengths().ToList();

            Func<List<int>, List<List<int>>> C = null;
            C = (input) => {
                List<List<int>> result = new List<List<int>>();
                //if (input.Count == 0) return result;
                if (input.Count == 1) return Enumerable.Range(0, input.First()).Select(x => new List<int>(new int[] { x })).ToList();

                for (int i = 0; i < input.First(); i++) {
                    var r = C(input.Skip(1).ToList()).Select(x => {
                        x.Insert(0, i);
                        return x;
                    });
                    result.AddRange(r);
                }
                return result;
            };

            return C(Indexes);
        }

        /// <summary>
        /// 設定多維<see cref="System.Array"/>中指定位置之元素的值。索引已指定為 32 位元整數的List
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <param name="value">指定元素的新值</param>
        /// <param name="index">32 位元整數的List，代表指定要設定之元素位置的索引</param>
        public static void SetValue(this Array obj, object value, List<int> index) {
            obj.SetValue(value, index.ToArray());
        }

        /// <summary>
        /// 取得多維<see cref="System.Array"/>中位於指定位置的值。索引已指定為 32 位元整數的List
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <param name="index">32 位元整數的清單，代表索引，指定要取得的<see cref="System.Array"/>元素的位置</param>
        /// <returns>元素值</returns>
        public static object GetValue(this Array obj, List<int> index) {
            return obj.GetValue(index.ToArray());
        }

        /// <summary>
        /// 將輸入陣列填滿指定的值
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <param name="value">指定數值</param>
        public static void Full(this Array obj, object value) {
            for (int i = 0; i < obj.Length; i++) {
                obj.SetValue(value, i);
            }
        }

        /// <summary>
        /// 提取陣列中指定的區段
        /// </summary>
        /// <typeparam name="T">陣列元素型別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">長度</param>
        /// <returns>提取區段</returns>
        public static Span<T> SpanSlice<T>(this T[] obj, int index, int length) {
            return obj.AsSpan().Slice(index, length);
        }

        /// <summary>
        /// 提取陣列中指定的區段
        /// </summary>
        /// <typeparam name="T">陣列元素型別</typeparam>
        /// <param name="obj">目前實例</param>
        /// <param name="index">起始索引</param>
        /// <returns>提取區段</returns>
        public static Span<T> SpanSlice<T>(this T[] obj, int index) {
            return obj.AsSpan().Slice(index);
        }
    }
}
