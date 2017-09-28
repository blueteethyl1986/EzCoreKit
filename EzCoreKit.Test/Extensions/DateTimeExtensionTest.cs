using EzCoreKit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class DateTimeExtensionTest {
        [Fact(DisplayName = "Extensions.DateTime.ToUnixTimestamp")]
        public void ToUnixTimestamp_Test() {
            Assert.Equal(DateTimeConvert.ToUnixTimestamp(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)), 0);
        }

        [Fact(DisplayName = "Extensions.DateTime.FromUnixTimestamp")]
        public void FromUnixTimestamp_Test() {
            Assert.Equal(DateTimeConvert.FromUnixTimestamp(0), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
