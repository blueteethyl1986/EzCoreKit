using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EzCoreKit.Cryptography {
    /// <summary>
    /// 雜湊計算幫助類別
    /// </summary>
    public static class HashHelper {
        /// <summary>
        /// 將字串使用指定的雜湊演算法轉換為雜湊
        /// </summary>
        /// <typeparam name="Algorithm">雜湊演算法型別</typeparam>
        /// <param name="value">值</param>
        /// <returns>雜湊Binary</returns>
        public static byte[] ToHash<Algorithm>(string value) where Algorithm : HashAlgorithm {
            using (var hash = typeof(Algorithm).GetMethod("Create", new Type[] { }).Invoke(null, null) as HashAlgorithm) {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        /// <summary>
        /// 將字串使用指定的雜湊演算法轉換為雜湊後在轉換為16進位字串表示
        /// </summary>
        /// <typeparam name="Algorithm">雜湊演算法型別</typeparam>
        /// <param name="value">值</param>
        /// <param name="upper">是否轉換為大寫</param>
        /// <returns>雜湊字串</returns>
        public static string ToHashString<Algorithm>(string value, bool upper = true) where Algorithm : HashAlgorithm {
            return string.Join("", ToHash<Algorithm>(value).Select(x => x.ToString(upper ? "X2" : "x2")));
        }
    }
}
