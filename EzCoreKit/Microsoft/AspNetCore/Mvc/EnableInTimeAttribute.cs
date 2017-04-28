using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Microsoft.AspNetCore.Mvc {
    /// <summary>
    /// 標誌控制器或方法在指定期間內啟用，其餘時間停用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EnableInTimeAttribute : Attribute {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public EnableInTimeAttribute(string StartTime,string EndTime) {
            Start = DateTime.Parse(StartTime);
            End = DateTime.Parse(EndTime);
        }
    }
}
