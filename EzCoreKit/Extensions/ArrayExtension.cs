using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class ArrayExtension {
        /// <summary>
        /// 取得代表 <see cref="System.Array"/> 所有維度之元素數目的 32 位元整數。
        /// </summary>
        /// <param name="target"></param>
        /// <returns>32 位元的整數陣列，代表所有維度的元素數目。</returns>
        public static int[] GetLengths(this Array target) {
            return Enumerable.Range(0, target.Rank).Select(x => target.GetLength(x)).ToArray();
        }

        /// <summary>
        /// 取得代表 <see cref="System.Array"/> 所有元素的索引。
        /// </summary>
        /// <param name="target"></param>
        /// <returns>32 位元的整數陣列的物件清單，代表所有元素的索引。</returns>
        public static List<List<int>> GetAllIndexes(this Array target) {
            List<int> Indexes = target.GetLengths().ToList();

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
        /// 設定多維 System.Array 中指定位置之元素的值。索引已指定為 32 位元整數的List。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value">指定元素的新值。</param>
        /// <param name="index">32 位元整數的List，代表指定要設定之元素位置的索引。</param>
        public static void SetValue(this Array obj, object value, List<int> index) {
            obj.SetValue(value, index.ToArray());
        }

        /// <summary>
        /// 取得多維 System.Array 中位於指定位置的值。索引已指定為 32 位元整數的List。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index">32 位元整數的清單，代表索引，指定要取得的 System.Array 元素的位置。</param>
        /// <returns></returns>
        public static object GetValue(this Array obj, List<int> index) {
            return obj.GetValue(index.ToArray());
        }
        
        /// <summary>
        /// 將輸入陣列填滿指定的值
        /// </summary>
        /// <param name="ary">目標陣列</param>
        /// <param name="value">指定數值</param>
        public static void Full(this Array ary, object value) {
            for (int i = 0; i < ary.Length; i++) {
                ary.SetValue(value, i);
            }
        }
        /// <summary>
        /// 求陣列數值最小公倍數
        /// </summary>
        public static int LCM(this int[] values) {
            int result = values[0];
            foreach (int value in values) {
                result = LCM(result, value);
            }
            return result;
        }

        /// <summary>
        /// 求兩數值之最小公倍數
        /// </summary>
        public static int LCM(int m, int n) {
            return m * n / GCD(m, n);
        }

        /// <summary>
        /// 求陣列數值之最大公因數
        /// </summary>
        /// <param name="ary">輸入陣列</param>
        public static int GCD(this int[] ary) {
            int result = ary[0];
            foreach (int value in ary)
                result = GCD(result, value);
            return result;
        }

        /// <summary>
        /// 求兩數值之最大公因數
        /// </summary>
        public static int GCD(int m, int n) {
            if (n == 0)
                return m;
            else
                return GCD(n, m % n);
        }
    }
}
