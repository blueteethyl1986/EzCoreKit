using EzCoreKit.Rest.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.Test.Rest {
    [RestBaseUri("http://opendata.cwb.gov.tw/")]
    public interface IFakeAPI {
        [RestMethod(
            Uri = "govdownload?dataid=E-A0015-001R&authorizationkey=rdec-key-123-45678-011121314")]
        Task<string> GetEarthquakesLocation();
    }
}
