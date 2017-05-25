using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.System.Extensions {
    public static class ArrayExtension {
        /// <summary>
        /// 將輸入陣列填滿指定的值
        /// </summary>
        /// <param name="Ary">目標陣列</param>
        /// <param name="Value">指定數值</param>
        public static void Full(this Array Ary, object Value) {
            for (int i = 0; i < Ary.Length; i++) {
                Ary.SetValue(Value, i);
            }
        }
        /// <summary>
        /// 求陣列數值最小公倍數
        /// </summary>
        public static int LCM(this int[] Values) {
            int result = Values[0];
            foreach (int value in Values) {
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
