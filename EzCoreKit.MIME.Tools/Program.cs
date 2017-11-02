using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EzCoreKit.MIME.Tools {
    public static partial class Program {

        static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length == 0) {
                args = new string[] { "data\\mime.xlsx" };
            };

            List<MIMEData> mimeList = new List<MIMEData>();

            using (ExcelPackage file = new ExcelPackage(new FileInfo(args[0]))) {
                var sheet = file.Workbook.Worksheets[1];

                for (int row = 2; ; row++) {
                    var mimeItem = new MIMEData();

                    mimeItem.Name = sheet.Cells[row, 1].Value?.ToString();
                    mimeItem.Summary = mimeItem.Name;
                    if (string.IsNullOrEmpty(mimeItem.Name)) {
                        break;
                    }

                    mimeItem.MIME = sheet.Cells[row, 2].Value.ToString();
                    mimeItem.FileExts = sheet.Cells[row, 3].Value.ToString()
                        .Split(',')
                        .Select(x => x.Trim())
                        .ToArray();

                    mimeList.Add(mimeItem);
                }
            }

            foreach (var list in mimeList.GroupBy(x => x.Type)) {
                MakeCsFile(list);
            }
        }

        private static void MakeCsFile(IGrouping<string, MIMEData> list) {
            var fileName = $"..\\EzCoreKit.MIME\\DeclareMIME.{FirstUpcase(list.First().Type)}.cs";
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            var write = File.CreateText(fileName);

            write.WriteLine("using System;");
            write.WriteLine("using EzCoreKit.MIME.Attributes;");
            write.WriteLine();
            write.WriteLine("namespace EzCoreKit.MIME {");
            write.WriteLine("\tpublic static partial class DeclareMIME {");
            var nList = list.Select(x => {
                x.Name = ConvertName(x.Name);
                return x;
            });
            foreach (var item in nList) {
                write.WriteLine("\t\t" + "/// <summary>");
                write.WriteLine("\t\t" + $"/// {item.Summary}");
                write.WriteLine("\t\t" + "/// </summary>");
                foreach (var ext in item.FileExts) {
                    write.WriteLine("\t\t" + $"[FileExtName(\"{ext}\")]");
                }
                if (list.Count(x => x.Name == item.Name) > 1) {
                    item.Name += "_" + item.Type + item.FileExts.First().Replace(".", "_");
                }
                write.WriteLine("\t\t" + $"public const string {item.Name} = \"{item.MIME}\";");
                write.WriteLine();
            }

            write.WriteLine("\t}");
            write.WriteLine("}");

            write.Close();
        }

        private static string FirstUpcase(string str) {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        private static string ConvertName(string name) {
            if (name[0] >= '0' && name[0] <= '9') {
                name = "_" + name;
            }

            Regex regex = new Regex("[^0-9a-zA-Z_]+");
            name = regex.Replace(name, "_");

            if (name.Last() == '_') {
                name = name.Substring(0, name.Length - 1);
            }

            return FirstUpcase(name);
        }
    }
}
