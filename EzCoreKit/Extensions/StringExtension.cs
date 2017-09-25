using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EzCoreKit.Extensions {
    public static class StringExtension {
        /// <summary>
        /// 安全的從這個執行個體擷取子字串。子字串會在指定的字元開始並繼續到字串結尾
        /// </summary>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">擷取子字串最長長度</param>
        /// <returns>子字串</returns>
        public static string SafeSubstring(this string target, int startIndex, int length) {
            if (target.Length >= startIndex) return string.Empty;
            string result = target.Substring(startIndex);
            length = Math.Min(result.Length, length);
            return result.Substring(0, length);
        }

        /// <summary>
        /// 檢查字串是否符合表示式
        /// </summary>
        /// <param name="regexString">正規表示式</param>
        public static bool IsMatch(this string target, string regexString) {
            Regex Regex = new Regex(regexString);
            return Regex.IsMatch(target);
        }

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
        public static string[] SplitByRegex(this string input, string pattern, int count) {
            return new Regex(pattern).Split(input, count);
        }

        /// <summary>
        /// 使用正規表示式切割字串
        /// </summary>
        /// <param name="input">值</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">數量</param>
        /// <param name="startat">起始索引</param>
        /// <returns>切割結果</returns>
        public static string[] SplitByRegex(this string input, string pattern, int count, int startat) {
            return new Regex(pattern).Split(input, count, startat);
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

        #region AutoSpace
        private static bool Between(this char obj, int min, int max) {
            return obj >= min && obj <= max;
        }

        private static Func<char, string>[] LangRanges = new Func<char, string>[] {
            (c)=> {//Unicode CJK範圍
                if(c.Between(0x2E80, 0x2EFF) ||
                   c.Between(0x3000, 0x303F) ||
                   c.Between(0x3200, 0x32FF) ||
                   c.Between(0x3300, 0x33FF) ||
                   c.Between(0x3400, 0x4DBF) ||
                   c.Between(0x4E00, 0x9FFF) ||
                   c.Between(0xF900, 0xFAFF) ||
                   c.Between(0xFE30, 0xFE4F) ||
                   c.Between(0x20000, 0x2A6DF) ||
                   c.Between(0x2F800, 0x2FA1F)) {
                    return "CJK";
                }
                return null;
            },
            (c)=> {
                return "other";
            }
        };

        private static string GetLangType(this char c) {
            foreach (var func in LangRanges) {
                var result = func(c);
                if (result != null) return result;
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 字串中不同語系文字中插入空白字元
        /// </summary>
        /// <param name="str">字串</param>
        /// <returns>自動加入空白後的字串</returns>
        public static string Spacing(this string str) {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < str.Length - 1; i++) {
                builder.Append(str[i]);
                if (str[i] == ' ' || str[i + 1] == ' ') continue;
                if (str[i].GetLangType() != str[i + 1].GetLangType()) {
                    builder.Append(' ');
                }
            }

            builder.Append(str.Last());
            return builder.ToString();
        }
    }
}