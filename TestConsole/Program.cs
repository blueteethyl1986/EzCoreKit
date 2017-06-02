using System;
using System.Linq;
using System.Text;

namespace TestConsole {
    class Program {
        static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.InputEncoding = Encoding.GetEncoding(0);
            Console.OutputEncoding = Encoding.GetEncoding(0);

            int i = 0;
            foreach(var news in new NkfustNews()) {
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