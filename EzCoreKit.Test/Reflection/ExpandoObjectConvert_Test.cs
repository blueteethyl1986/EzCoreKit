using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Reflection;
using System.Dynamic;
using EzCoreKit.Test.TestModels;
using Xunit;

namespace EzCoreKit.Test.Reflection {
    public class ExpandoObjectConvert_Test {
        [Fact(DisplayName = "Extensions.Reflection.ConvertToExpando")]
        public void ConvertToExpando_Test() {
            dynamic obj1 = new ExpandoObject();
            obj1.Id = 12;
            obj1.Name = "name";
            obj1.Class = "class";

            dynamic obj2 = ExpandoObjectConvert.ConvertToExpando(new Student() {
                Id = 12,
                Name = "name",
                Class = "class"
            });

            Assert.NotNull(obj1 as IDictionary<string, object>);
            Assert.NotNull(obj2 as IDictionary<string, object>);
            Assert.Equal(
                obj1 as IDictionary<string, object>,
                obj2 as IDictionary<string, object>);
        }
    }
}
