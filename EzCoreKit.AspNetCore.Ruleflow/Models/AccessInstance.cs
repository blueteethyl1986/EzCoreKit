using EzCoreKit.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzCoreKit.AspNetCore.Ruleflow.Models {
    /// <summary>
    /// 存取實體
    /// </summary>
    public class AccessInstance {
        /// <summary>
        /// 唯一識別號
        /// </summary>
        
        public string Id { get; set; }

        /// <summary>
        /// 金鑰
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 規則鏈進入點
        /// </summary>
        public string RuleEnterId { get; set; }
        
    }
}
