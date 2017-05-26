using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 標誌停用的控制器或方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableAttribute : Attribute {
        
    }
}
