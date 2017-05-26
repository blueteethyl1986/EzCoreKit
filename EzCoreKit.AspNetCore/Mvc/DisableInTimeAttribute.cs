using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 標誌控制器或方法在指定期間內停用，其餘時間啟用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =true)]
    public class DisableInTimeAttribute : Attribute {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DisableInTimeAttribute(string StartTime, string EndTime) {
            Start = DateTime.Parse(StartTime);
            End = DateTime.Parse(EndTime);
        }
    }
}
