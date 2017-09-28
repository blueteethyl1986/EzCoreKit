using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using EzCoreKit.Reflection;
using Xunit;
using System.Linq;

namespace EzCoreKit.Test.Reflection {
    public class ExpandoObjectExtension_Test {
        [Fact(DisplayName = "Extensions.Reflection.CreateAnonymousType")]
        public void CreateAnonymousType_Test() {
            dynamic obj1 = new ExpandoObject();
            obj1.Id = 12;
            obj1.Name = "name";
            obj1.Class = "class";

            var expObj = (ExpandoObject)obj1;

            Assert.Equal(
                expObj.CreateAnonymousType().GetProperties().Select(x => x.Name).ToArray(),
                new string[] { "Id", "Name", "Class" });
        }
    }
}
