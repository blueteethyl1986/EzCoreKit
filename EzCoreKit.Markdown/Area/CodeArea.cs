﻿using EzCoreKit.Markdown.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EzCoreKit.Markdown.Area {
    /// <summary>
    /// 程式碼區塊
    /// </summary>
    [Match(Regex = "^```.+(\r?\n((?!```)(.|.+```))*)+\r?\n```\r?\n")]
    public class CodeArea : MarkdownRaw {
        /// <summary>
        /// 語言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 程式碼
        /// </summary>
        public string Code { get; set; }

        public override string OuterMarkdown {
            get {
                return "```" + Language + "\n" +
                    Code + "\n```";
            }
            set {
                base.OuterMarkdown = value;
            }
        }

        public override string InnerMarkdown {
            get {
                return Code;
            }
        }

        public override string OuterText {
            get {
                return Code;
            }
        }

        public override string InnerText {
            get {
                return Code;
            }
        }

        public static CodeArea Parse(string text, out int length) {
            var attrs = MatchAttribute.GetMatchAttributes<CodeArea>()
                .Select(x => new {
                    match = x.GetRegex().IsMatch(text),
                    attr = x
                });

            var result = new CodeArea();
            Match match = attrs.Where(x => x.match)
                .FirstOrDefault().attr
                .GetRegex().Match(text);

            var temp = match.Value.Replace("\r", "").Trim();
            Match langMatch = new Regex(@"```.+\r?\n").Match(temp);

            result.Language = langMatch.Value.Substring(3);
            result.Language = result.Language.Substring(0, result.Language.Length - 1);

            result.Code = temp.Substring(langMatch.Index + langMatch.Length);
            result.Code = result.Code.Substring(0, result.Code.Length - 3);

            var codeLines = result.Code.Split('\n');
            if (codeLines.LastOrDefault()?.Length == 0) {
                result.Code =
                    string.Join("\n", codeLines.Take(codeLines.Length - 1));
            }

            length = match.Index + match.Length;
            return result;
        }
    }
}
