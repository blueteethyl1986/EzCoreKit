using EzCoreKit.Rest.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.Test.Rest {
    [RestBaseUri("https://jsonplaceholder.typicode.com/")]
    public interface IFakeAPI {
        [RestMethod(
            Uri = "http://opendata.cwb.gov.tw/govdownload?dataid=E-A0015-001R&authorizationkey=rdec-key-123-45678-011121314")]
        Task<string> GetEarthquakesLocation();
    }
}
