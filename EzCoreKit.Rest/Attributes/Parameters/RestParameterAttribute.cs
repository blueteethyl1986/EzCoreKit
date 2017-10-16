using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Rest.Attributes.Parameters {
    public class RestParameterAttribute : Attribute {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
