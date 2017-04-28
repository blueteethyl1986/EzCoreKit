using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EzCoreKit.System.Security.Cryptography {
    public static class StringHashExtension {
        /// <summary>
        /// 將字串使用指定的雜湊演算法轉換為雜湊
        /// </summary>
        /// <typeparam name="Algorithm">雜湊演算法型別</typeparam>
        /// <param name="value">值</param>
        /// <returns>雜湊Binary</returns>
        public static byte[] ToHash<Algorithm>(this string value) where Algorithm : HashAlgorithm {
            return HashFactory.ToHash<Algorithm>(value);
        }

        /// <summary>
        /// 將字串使用指定的雜湊演算法轉換為雜湊後在轉換為16進位字串表示
        /// </summary>
        /// <typeparam name="Algorithm">雜湊演算法型別</typeparam>
        /// <param name="value">值</param>
        /// <param name="upper">是否轉換為大寫</param>
        /// <returns>雜湊字串</returns>
        public static string ToHashString<Algorithm>(this string value, bool upper = true) where Algorithm : HashAlgorithm {
            return HashFactory.ToHashString<Algorithm>(value, upper);
        }
    }
}
