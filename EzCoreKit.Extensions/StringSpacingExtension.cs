using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.Extensions {
    /// <summary>
    /// 針對<see cref="String"/>不同語系插入空白字符擴充方法
    /// </summary>
    public static partial class StringSpacingExtension {
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


        /// <summary>
        /// 字串中不同語系文字中插入空白字元
        /// </summary>
        /// <param name="obj">目前實例</param>
        /// <returns>自動加入空白後的字串</returns>
        public static string Spacing(this string obj) {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < obj.Length - 1; i++) {
                builder.Append(obj[i]);
                if (obj[i] == ' ' || obj[i + 1] == ' ') continue;
                if (obj[i].GetLangType() != obj[i + 1].GetLangType()) {
                    builder.Append(' ');
                }
            }

            builder.Append(obj.Last());
            return builder.ToString();
        }
    }
}
