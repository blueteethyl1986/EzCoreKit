using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Ruleflow.Exceptions {
    public class AccessRuleNotFoundException : Exception {
        public AccessRuleNotFoundException() : base("找不到指定Rule實體") { }
    }
}
