using EzCoreKit.Test.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Reflection;
using Xunit;
using System.Linq;

namespace EzCoreKit.Test.Reflection {
    public class TypeHelperExtension_Test {
        [Fact(DisplayName = "Reflection.GetNamespaceTypes")]
        public void GetNamespaceTypes() {
            var types = TypeHelper.GetNamespaceTypes("EzCoreKit.Test.TestModels")
                .Where(x => x.IsClass);
            Assert.NotEmpty(types);
        }
    }
}
