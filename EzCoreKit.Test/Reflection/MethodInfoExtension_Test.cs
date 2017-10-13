using System;
using System.Collections.Generic;
using EzCoreKit.Reflection;
using System.Text;
using Xunit;
using EzCoreKit.Test.TestModels;
using System.Reflection;

namespace EzCoreKit.Test.Reflection {
    public class MethodInfoExtension_Test {
        [Fact(DisplayName = "Reflection.MethodInfo_InvokeAndDelegate")]
        public void MethodInfo_InvokeAndDelegate_Test() {
            var obj = new Student() {
                Id = 8,
                Class = "B",
                Name = "XPY"
            };

            var methodInfo1 = obj.GetMember(x => Student.TestMethod()) as MethodInfo;
            var methodInfo2 = obj.GetMember(x => x.ToString()) as MethodInfo;

            Assert.Equal(methodInfo1.Invoke(), Student.TestMethod());
            Assert.Equal(methodInfo2.ToDelegate(obj)(null), obj.ToString());
        }
    }
}
