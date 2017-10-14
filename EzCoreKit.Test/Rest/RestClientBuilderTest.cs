using EzCoreKit.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Rest {
    public class RestClientBuilderTest {
        [Fact(DisplayName = "Rest.CreateInstance")]
        public async void RestClientBuilder_CreateInstanceTest() {
            RestClientBuilder<IFakeAPI> builder = new RestClientBuilder<IFakeAPI>();
            var obj = builder.Build();
            Assert.NotNull(obj);
        }

        [Fact(DisplayName = "Rest.CallInstanceMethod")]
        public async void RestClientBuilder_CallInstanceMethodTest() {
            RestClientBuilder<IFakeAPI> builder = new RestClientBuilder<IFakeAPI>();
            var obj = builder.Build();
            var result = await obj.GetEarthquakesLocation();
        }
    }
}
