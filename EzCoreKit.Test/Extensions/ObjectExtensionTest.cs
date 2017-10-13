using EzCoreKit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class ObjectExtensionTest {
        [Fact(DisplayName = "Extensions.Object.BoxingToLazy")]
        public void GetEnumNameExtension_Test() {
            Assert.Equal("ABC".BoxingToLazy().Value, "ABC");
        }
    }
}
