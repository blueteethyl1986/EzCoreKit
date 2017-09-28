using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Extensions;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class TypeExtensionTest {
        [Fact(DisplayName = "Extensions.Type.IsAnonymousType")]
        public void IsAnonymousType_Test() {
            Assert.Equal(new { }.GetType().IsAnonymousType(), true);
            Assert.Equal("".GetType().IsAnonymousType(), false);
        }

        [Fact(DisplayName = "Extensions.Type.GetDefalut")]
        public void GetDefault_Test() {
            Assert.Equal(new { }.GetDefault(), null);
            Assert.Equal(1.GetDefault(), 0);
        }
    }
}