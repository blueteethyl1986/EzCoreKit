using System;
using System.Collections.Generic;
using EzCoreKit.Reflection;
using System.Text;
using Xunit;
using EzCoreKit.Test.TestModels;

namespace EzCoreKit.Test.Reflection {
    public class AccessExpressionFactory_Test {
        [Fact(DisplayName = "Reflection.CreateAccessFunc")]
        public void CreateAccessFunc() {
            var obj = new Student() { Name = "HaHa" };
            var func = AccessExpressionFactory.CreateAccessFunc<Student>("Name");

            Assert.Equal(func(obj), obj.Name);
        }

        [Fact(DisplayName = "Reflection.CreateAccessExpressionFunc")]
        public void CreateAccessFuncExpression() {
            var obj = new Student() { Name = "HaHa" };
            var expFunc = AccessExpressionFactory.CreateAccessExpressionFunc<Student>("Name");

            Assert.Equal(expFunc.Compile().Invoke(obj), obj.Name);
        }
    }
}
