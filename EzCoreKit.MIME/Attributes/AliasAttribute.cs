using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.MIME.Attributes {
    /// <summary>
    /// 表示MIME變數別名
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class AliasAttribute : Attribute {
        /// <summary>
        /// 別名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 初始化MIME變數別名
        /// </summary>
        /// <param name="name">別名</param>
        public AliasAttribute(string name) {
            this.Name = name;
        }
    }
}
