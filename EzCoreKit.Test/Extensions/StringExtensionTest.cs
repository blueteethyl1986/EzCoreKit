using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Extensions;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class StringExtensionTest {
        [Theory(DisplayName = "Extensions.String.SafeSubstring")]
        [InlineData(new object[] { "0123456789", 0, 3, "012" })]
        [InlineData(new object[] { "0123456789", 1, 0, "" })]
        [InlineData(new object[] { "0123456789", 9, 100, "9" })]
        [InlineData(new object[] { "0123456789", 999, 100, "" })]
        [InlineData(new object[] { "0123456789", -1, 3, "012" })]
        [InlineData(new object[] { "0123456789", -80, -1, "" })]
        public void SafeSubstring_Test(string input, int index, int length, string result) {
            Assert.Equal(input.SafeSubstring(index, length), result);
        }

        [Theory(DisplayName = "Extensions.String.InnerString")]
        [InlineData(new object[] { "0123456789", "1", "5", "234" })]
        [InlineData(new object[] { "0123456789", "0", "1", "" })]
        [InlineData(new object[] { "0123456789", "A", "B", "" })]
        [InlineData(new object[] { "0123456789", "5", "1", "" })]
        public void InnerString_Test(string input, string start, string end, string result) {
            Assert.Equal(input.InnerString(start, end), result);
        }
    }
}
