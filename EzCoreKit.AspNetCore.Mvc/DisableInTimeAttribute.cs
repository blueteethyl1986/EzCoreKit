using System;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 標誌控制器或方法在指定期間內停用，其餘時間啟用(大於等於Start並且小於End)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DisableInTimeAttribute : Attribute {
        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime End { get; set; }

        public DisableInTimeAttribute(string StartTime, string EndTime) {
            Start = DateTime.Parse(StartTime);
            End = DateTime.Parse(EndTime);
        }
    }
}