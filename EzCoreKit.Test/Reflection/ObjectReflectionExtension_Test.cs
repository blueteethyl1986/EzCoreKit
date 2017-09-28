using EzCoreKit.Test.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Reflection;
using Xunit;

namespace EzCoreKit.Test.Reflection {
    public class ObjectReflectionExtension_Test {
        [Fact(DisplayName = "Extensions.Reflection.GetPrivateFieldValue")]
        public void GetPrivateFieldValue() {
            Assert.Equal(
                new Student().GetPrivateFieldValue<string>("TestPrivateField"),
                "abc");
            Assert.Equal(
                new Student().GetPrivateFieldValue<string>("TestInternalField"),
                "def");
        }
    }
}
