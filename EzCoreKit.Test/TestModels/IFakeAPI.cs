using EzCoreKit.Rest.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using EzCoreKit.Rest.Attributes.Paramters;

namespace EzCoreKit.Test.TestModels {
    [RestBaseUri("https://jsonplaceholder.typicode.com/")]
    public interface IFakeAPI {
        [RestMethod(//Default Method=HttpMethods.Get
            Uri = "posts",
            ResponseFormat = DataFormat.Json)]
        Task<Post[]> GetPosts([RestQueryParameter]int? userId = null);

        [RestMethod(//Default Method=HttpMethods.Get
            Uri = "comments",
            ResponseFormat = DataFormat.Json)]
        Task<Comment[]> GetComments([RestQueryParameter]int postId);

        [RestMethod(
            Uri = "http://opendata.cwb.gov.tw/govdownload?dataid=E-A0015-001R&authorizationkey=rdec-key-123-45678-011121314",
            ResponseFormat = DataFormat.Xml,
            Path = "//location/text()")]
        Task<string> GetEarthquakesLocation();

        [RestMethod(
            Uri = "https://data.wra.gov.tw/Service/OpenData.aspx?format=xml&id=D54BA676-ED9A-4077-9A10-A0971B3B020C",
            ResponseFormat = DataFormat.Xml,
            Path = "//ReservoirName/text()")]
        Task<string[]> GetReservoirsName();

        [RestMethod(
            Uri = "https://data.wra.gov.tw/Service/OpenData.aspx?format=xml&id=D54BA676-ED9A-4077-9A10-A0971B3B020C",
            ResponseFormat = DataFormat.Xml,
            Path = "//ReservoirsInformation")]
        Task<Reservoirs[]> GetReservoirs();
    }
}
