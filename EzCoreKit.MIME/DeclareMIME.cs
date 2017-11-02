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

        /// <summary>
        /// 使用副檔名查詢對應的MIME名稱
        /// </summary>
        /// <param name="fileExt">副檔名(包含開頭點)</param>
        /// <returns>MIME</returns>
        public static string GetMIMENameByFileExt(string fileExt) {
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            fileExt = fileExt.ToLower();
            foreach (var field in fields) {
                var fileExts = field.GetCustomAttributes<FileExtNameAttribute>();
                if (fileExts.Any(x => x.FileExtension == fileExt)) {
                    return field.Name;
                }
            }
            return null;
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
                if(field.Name.ToLower() == name ||
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
        public static List<string> GetMIMENameList() {
            List<string> result = new List<string>();
            var fields = typeof(DeclareMIME).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach(var field in fields) {
                result.Add(field.Name);
            }
            return result;
        }
    }
}
