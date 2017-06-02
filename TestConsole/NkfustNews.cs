using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace TestConsole {
    public class NewsFile {
        public string Name { get;internal set; }
        public string Url { get; internal set; }
        public string Size { get; internal set; }
    }

    public class News {
        public string Title { get;internal set; }
        public string Url { get; internal set; }
        public string Date { get; internal set; }
        public string Author { get; internal set; }
        public bool NoContent {
            get {
                return Url.Contains("ezfiles");
            }
        }
        public Lazy<HtmlDocument> _document;
        public News() {
            //document.querySelector(".ptcontent").innerText
            _document = new Lazy<HtmlDocument>(this.LazyLoad);
        }

        private HtmlDocument LazyLoad() {
            if (NoContent) return null;
            HtmlWeb webClient = new HtmlWeb();
            return webClient.Load(Url);            
        }

        public string ContentText {
            get {
                var result = _document.Value
                    .DocumentNode.SelectSingleNode(".//div[contains(@class,'ptcontent')]")
                    ?.InnerText?.Trim();
                if (result == null) return null;
                return System.Net.WebUtility.HtmlDecode(result);
            }
        }

        public string ContentHtml {
            get {
                return _document.Value
                    .DocumentNode.SelectSingleNode(".//div[contains(@class,'ptcontent')]")
                    ?.InnerHtml;
            }
        }

        public NewsFile[] Files {
            get {
                var fileNodes =
                    _document.Value
                    .DocumentNode.SelectNodes(".//table[contains(@class,'baseTB')]/tr");
                
                List<NewsFile> result = new List<NewsFile>();

                if (fileNodes != null) {
                    foreach (var fileNode in fileNodes) {
                        var urlNode = fileNode.SelectSingleNode(".//a");
                        result.Add(new NewsFile() {
                            Url = urlNode.Attributes["href"].Value,
                            Name = urlNode.InnerText,
                            Size = fileNode.SelectSingleNode(".//span[3]").InnerText
                        });
                    }
                }

                return result.ToArray();

            }
        }
    }

    public class NkfustNews : EzCoreKit.Spider.PageLoaderBase<News> {
        private string urlTempleate = "http://www.nkfust.edu.tw/files/501-1000-1004-{0}.php?Lang=zh-tw";
        protected override News[] LoadPage(int pageIndex) {
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument html = webClient.Load(string.Format(urlTempleate, pageIndex + 1));
            var newsRows = html.DocumentNode.SelectNodes("//td[contains(@class,'mc')]/div[contains(@class,'h5')]");

            List<News> result = new List<News>();
            foreach(var row in newsRows) {
                var dateNode = row.SelectSingleNode("./span[contains(@class,'date')]");
                if (dateNode == null) continue;

                var titleNode = row.SelectSingleNode("./a");

                result.Add(new News() {
                    Title = titleNode.InnerText,
                    Url = titleNode.Attributes["href"].Value,
                    Date = dateNode.InnerText.Replace(" ", "").Replace("[", "").Replace("]", ""),
                    Author = row.SelectSingleNode("./span[contains(@class,'subsitename')]").InnerText
                });
            }

            return result.ToArray();
        }
    }
}
