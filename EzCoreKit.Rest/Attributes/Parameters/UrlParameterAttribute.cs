using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Rest.Attributes.Paramters {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = true)]
    public class UrlParameterAttribute : Attribute {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
