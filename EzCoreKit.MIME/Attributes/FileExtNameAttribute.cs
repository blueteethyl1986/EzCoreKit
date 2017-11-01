using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.MIME.Attributes {
    /// <summary>
    /// 表示對應的MIME副檔名
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class FileExtNameAttribute : Attribute {
        /// <summary>
        /// 副檔名(包含開頭的點)
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 初始化MIME副檔名
        /// </summary>
        /// <param name="fileExtension">副檔名(包含開頭的點)</param>
        public FileExtNameAttribute(string fileExtension) {
            this.FileExtension = fileExtension;
        }
    }
}
