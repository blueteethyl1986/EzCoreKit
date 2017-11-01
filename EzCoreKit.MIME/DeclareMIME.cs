using EzCoreKit.MIME.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace EzCoreKit.MIME {
    /// <summary>
    /// MIME對應類別
    /// </summary>
    public partial class DeclareMIME {
        /// <summary>
        /// 使用副檔名查詢對應的MIME
        /// </summary>
        /// <param name="fileExt">副檔名(包含開頭點)</param>
        /// <returns>MIME</returns>
        public static string GetMIMEByFileExt(string fileExt) {
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            fileExt = fileExt.ToLower();
            foreach (var field in fields) {
                var fileExts = field.GetCustomAttributes<FileExtNameAttribute>();
                if (fileExts.Any(x => x.FileExtension == fileExt)) {
                    return (string)field.GetValue(null);
                }
            }
            return null;
        }
    }
}
