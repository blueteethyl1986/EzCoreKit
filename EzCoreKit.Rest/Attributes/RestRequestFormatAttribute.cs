using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Rest.Attributes {
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Interface)]
    public class RestRequestFormatAttribute : Attribute {
        public DataFormat Format { get; set; }

        public RestRequestFormatAttribute(DataFormat format) {
            Format = format;
        }
    }
}
