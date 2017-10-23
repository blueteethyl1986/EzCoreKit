using EzCoreKit.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using EzCoreKit.Test.TestModels;

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
            var temp = await obj.GetPosts();

            Assert.NotEmpty(temp);

            var result = await obj.GetEarthquakesLocation();
            Assert.NotNull(result);
        }
    }
}
