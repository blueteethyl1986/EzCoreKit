using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Threading {
    /// <summary>
    /// Promise狀態
    /// </summary>
    public enum PromiseStatus {
        /// <summary>
        /// 擱置
        /// </summary>
        Pending,
        /// <summary>
        /// 完成
        /// </summary>
        Fulfilled,
        /// <summary>
        /// 失敗
        /// </summary>
        Rejected
    }
}
