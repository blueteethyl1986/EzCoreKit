﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EzCoreKit.Markdown.Line;
using EzCoreKit.Markdown.Attributes;

namespace EzCoreKit.Markdown.Inline {
    /// <summary>
    /// 連結
    /// </summary>
    [Match(Regex = @"^\[.+\]\s*\[.+\]")]
    [Match(Regex = @"^\[.+\]\s*\(.+\)")]
    public class Link : MarkdownRaw {
        /// <summary>
        /// 文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 連結
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 參考標籤
        /// </summary>
        public Tag ReferenceTag { get; set; }

        public override string OuterMarkdown {
            get {
                string result = $"[{Text}]";
                if (string.IsNullOrWhiteSpace(URL)) {
                    result += $"[{ReferenceTag?.Id}]";
                } else {
                    result += $"({URL})";
                }
                return result;
            }
            set {
                int temp = 0;
                Link autoLink = Parse(value, out temp);
                this.Text = autoLink.Text;
                this.URL = autoLink.URL;
                this.ReferenceTag = autoLink.ReferenceTag;
            }
        }

        public override string OuterText {
            get {
                return Text;
            }
        }

        public static Link Parse(string text,out int length ) {
            var attrs = MatchAttribute.GetMatchAttributes<Link>()
                .Select(x => new {
                    match = x.GetRegex().IsMatch(text),
                    attr = x
                });

            if (!attrs.Any(x => x.match)) throw new FormatException();

            try {
                Regex tagLink = new Regex(@"\[[^\]]+\]\s*\[[^\]]+\]");
                Regex urlLink = new Regex(@"\[[^\]]+\]\s*\([^\)]+\)");

                string linkText = tagLink.IsMatch(text) ?
                    tagLink.Match(text)?.Value :
                    urlLink.Match(text)?.Value;

                Regex t1Regex = new Regex(@"\[[^\]]+\]");
                Regex t2Regex = new Regex(@"\(.+\)");
                Link result = new Link();
                result.Text = t1Regex.Match(linkText).Value;
                result.Text = result.Text.Substring(1, result.Text.Length - 2);
                length = 0;
                if (tagLink.IsMatch(text)) {
                    var match = t1Regex.Match(linkText).NextMatch();
                    string tagId = match.Value;
                    tagId = tagId.Substring(1, tagId.Length - 2);

                    result.ReferenceTag = new Tag() { Id = tagId.Trim(), IsRef = true };
                    length = match.Index + match.Length;
                } else if (urlLink.IsMatch(text)) {
                    var match = t2Regex.Match(linkText);
                    string url = t2Regex.Match(linkText).Value;
                    url = url.Substring(1, url.Length - 2);

                    result.URL = url.Trim();
                    length = match.Index + match.Length;
                }
                return result;
            }catch(Exception e) {
                throw new FormatException();
            }
        }
    }
}
