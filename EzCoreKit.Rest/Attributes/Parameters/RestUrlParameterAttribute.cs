using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Rest.Attributes.Paramters {
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Parameter)]
    public class RestUrlParameterAttribute : Attribute {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
