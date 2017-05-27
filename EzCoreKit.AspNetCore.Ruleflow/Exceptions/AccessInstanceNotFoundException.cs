using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Ruleflow.Exceptions {
    public class AccessInstanceNotFoundException: Exception {
        public AccessInstanceNotFoundException() : base("找不到指定appId實體") { }
    }
}
