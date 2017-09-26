using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using EzCoreKit.Extensions;

namespace TestConsole {
    public class Student {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Score { get; set; }
    }

    class Program {

        static void Main(string[] args) {

            List<Student> list = new List<Student>();
            list.Add(new Student() {
                Name = "User1",
                Class = "A",
                Score = 56
            });
            list.Add(new Student() {
                Name = "User2",
                Class = "A",
                Score = 80
            });
            list.Add(new Student() {
                Name = "User3",
                Class = "B",
                Score = 90
            });
            list.Add(new Student() {
                Name = "User3",
                Class = "B",
                Score = 100
            });

            dynamic obj = new ExpandoObject();
            var dobj = obj as IDictionary<string, object>;
            dobj["Class"] = 0;
            dobj["User"] = "";

            var type = (obj as ExpandoObject).CreateAnonymousType();

            //var k = LinqExtension.GroupBy<Student, string>(list, new string[] { "Class", "Name" });



            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.InputEncoding = Encoding.GetEncoding(0);
            Console.OutputEncoding = Encoding.GetEncoding(0);

            int i = 0;
            foreach (var news in new NkfustNews()) {
                if (i++ == 10) break;
                Console.WriteLine($"{news.Date} - {news.Author} - {news.Title}");
                if (!news.NoContent) {
                    Console.WriteLine(news.ContentText);
                }
                Console.WriteLine("==========");
            }
        }
    }
}