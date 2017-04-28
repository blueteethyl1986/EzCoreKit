using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EzCoreKit.System.Extensions {
    public static class StringExtension {
        /// <summary>
        /// 使用正規表示式切割字串
        /// </summary>
        /// <param name="input">值</param>
        /// <param name="pattern">模式</param>
        /// <returns>切割結果</returns>
        public static string[] SplitByRegex(this string input, string pattern) {
            return new Regex(pattern).Split(input);
        }

        /// <summary>
        /// 使用正規表示式切割字串
        /// </summary>
        /// <param name="input">值</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">數量</param>
        /// <returns>切割結果</returns>
        public static string[] SplitByRegex(this string input, string pattern,int count) {
            return new Regex(pattern).Split(input,count);
        }

        /// <summary>
        /// 使用正規表示式切割字串
        /// </summary>
        /// <param name="input">值</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">數量</param>
        /// <param name="startat">起始索引</param>
        /// <returns>切割結果</returns>
        public static string[] SplitByRegex(this string input, string pattern,int count,int startat) {
            return new Regex(pattern).Split(input,count,startat);
        }

        /// <summary>
        /// 取得指定字串間的字串
        /// </summary>
        /// <param name="input">值</param>
        /// <param name="start">起始字串</param>
        /// <param name="end">結束字串</param>
        /// <returns>字串間的字串</returns>
        public static string InnerString(this string input, string start, string end) {
            string result = input.Substring(input.IndexOf(start) + start.Length);
            return result.Substring(0, result.IndexOf(end));
        }
    }
}