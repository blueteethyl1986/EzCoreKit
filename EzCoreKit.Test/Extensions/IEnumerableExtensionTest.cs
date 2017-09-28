using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Extensions;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class IEnumerableExtensionTest {
        [Fact(DisplayName = "Extensions.IEnumerable.IsEmpty")]
        public void IsEmpty_Test() {
            var emptyList = new List<int>();
            var notEmptyList = new List<int>(new int[] { 1 });

            Assert.Equal(emptyList.IsEmpty(), true);
            Assert.Equal(notEmptyList.IsEmpty(), false);
        }

        [Fact(DisplayName = "Extensions.IEnumerable.IsNotEmpty")]
        public void IsNotEmpty_Test() {
            var emptyList = new List<int>();
            var notEmptyList = new List<int>(new int[] { 1 });

            Assert.Equal(emptyList.IsNotEmpty(), false);
            Assert.Equal(notEmptyList.IsNotEmpty(), true);
        }
    }
}
