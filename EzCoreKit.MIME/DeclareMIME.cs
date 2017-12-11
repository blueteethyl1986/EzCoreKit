using EzCoreKit.MIME.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace EzCoreKit.MIME {
    /// <summary>
    /// 提供MIME對應與查詢類別
    /// </summary>
    public static partial class DeclareMIME {
        /// <summary>
        /// 使用副檔名查詢對應的MIME
        /// </summary>
        /// <param name="fileExt">副檔名(包含開頭點)</param>
        /// <returns>MIME</returns>
        public static string[] GetMIMEByFileExt(string fileExt) {
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            fileExt = fileExt.ToLower();
            var result = new List<string>();
            foreach (var field in fields) {
                var fileExts = field.GetCustomAttributes<FileExtNameAttribute>();
                if (fileExts.Any(x => x.FileExtension == fileExt)) {
                    result.Add((string)field.GetValue(null));
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 使用副檔名查詢對應的MIME名稱
        /// </summary>
        /// <param name="fileExt">副檔名(包含開頭點)</param>
        /// <returns>MIME</returns>
        public static string[] GetMIMENameByFileExt(string fileExt) {
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            fileExt = fileExt.ToLower();
            var result = new List<string>();
            foreach (var field in fields) {
                var fileExts = field.GetCustomAttributes<FileExtNameAttribute>();
                if (fileExts.Any(x => x.FileExtension == fileExt)) {
                    result.Add(field.Name);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 使用MIME名稱查詢對應的MIME
        /// </summary>
        /// <param name="name">MIME迷稱</param>
        /// <returns>MIME</returns>
        public static string GetMIMEByName(string name) {
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            name = name.ToLower();
            foreach (var field in fields) {
                if (field.Name.ToLower() == name ||
                   field.GetCustomAttributes<AliasAttribute>().Any(x => x.Name.ToLower() == name)) {
                    return (string)field.GetValue(null);
                }
            }
            return null;
        }

        /// <summary>
        /// 取得<see cref="DeclareMIME"/>類別內定義的所有MIME名稱列表
        /// </summary>
        /// <returns>MIME名稱列表</returns>
        public static string[] GetMIMENameList() {
            List<string> result = new List<string>();
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var field in fields) {
                result.Add(field.Name);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 取得指定類型的MIME名稱列表
        /// </summary>
        /// <returns>MIME名稱列表</returns>
        public static string[] GetMIMENameListByType(string type) {
            List<string> result = new List<string>();
            type = type.ToLower();
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var field in fields) {
                var typeString = (field.GetValue(null) as string)?.Split('/')?.FirstOrDefault();
                if (typeString == type) {
                    result.Add(field.Name);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 使用MIME反查所有MIME名稱
        /// </summary>
        /// <param name="mime">MIME</param>
        /// <returns>MIME名稱列表</returns>
        public static string[] GetMIMENameByMIME(string mime) {
            var result = new List<string>();
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var field in fields) {
                if (mime.Equals(field.GetValue(null))) {
                    result.Add(field.Name);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 使用MIME反查所有副檔名
        /// </summary>
        /// <param name="mime">MIME</param>
        /// <returns>副檔名列表</returns>
        public static string[] GetFileExtByMIME(string mime) {
            var result = new List<string>();
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var field in fields) {
                if (mime.Equals(field.GetValue(null))) {
                    result.AddRange(
                        field.GetCustomAttributes<FileExtNameAttribute>().Select(x => x.FileExtension)
                    );
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 使用MIME反查所有副檔名
        /// </summary>
        /// <param name="name">MIME</param>
        /// <returns>副檔名列表</returns>
        public static string[] GetFileExtByMIMEName(string name) {
            var field = typeof(DeclareMIME).GetField(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            return field.GetCustomAttributes<FileExtNameAttribute>().Select(x => x.FileExtension).ToArray();
        }

        /// <summary>
        /// 常見圖片類型MIME
        /// </summary>
        public static string[] CommomImageMIMEs {
            get {
                return new string[] {
                    JPEG_Image,
                    Graphics_Interchange_Format,
                    Portable_Network_Graphics_PNG,
                    WAP_Bitamp_WBMP,
                    Tagged_Image_File_Format,
                    JPEG_2000_Compound_Image_File_Format,
                    FlashPix
                };
            }
        }

        /// <summary>
        /// 常見圖片類型副檔名
        /// </summary>
        public static string[] GetCommomImageFileExts {
            get {
                return CommomImageMIMEs
                    .SelectMany(x => GetFileExtByMIME(x))
                    .ToArray();
            }
        }
    }
}
