using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Rest.Attributes.Parameters {
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Parameter)]
    public class RestFormParameterAttribute : RestParameterAttribute {
    }
}
