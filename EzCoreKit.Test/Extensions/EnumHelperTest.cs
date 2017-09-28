using EzCoreKit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class EnumHelperTest {
        private class TestAttribute : Attribute {
            public string Tag { get; set; }
        }
        private enum TestEnum {
            [Test(Tag = "A")]
            A,

            [Test(Tag = "B")]
            B,

            [Test(Tag = "C")]
            C,

            [Test(Tag = "D")]
            D
        }

        [Fact(DisplayName = "Extensions.Enum.GetEnumName")]
        public void GetEnumName_Test() {
            Assert.Equal("A", EnumHelper.GetEnumName(TestEnum.A));
            Assert.Equal("B", EnumHelper.GetEnumName(TestEnum.B));
            Assert.Equal("C", EnumHelper.GetEnumName(TestEnum.C));
            Assert.Equal("D", EnumHelper.GetEnumName(TestEnum.D));
        }

        [Fact(DisplayName = "Extensions.Enum.GetCustomAttributes")]
        public void GetCustomAttributes_Test() {
            Assert.Equal("A", EnumHelper.GetCustomAttribute<TestAttribute>(TestEnum.A).Tag);
            Assert.Equal("B", EnumHelper.GetCustomAttribute<TestAttribute>(TestEnum.B).Tag);
            Assert.Equal("C", EnumHelper.GetCustomAttribute<TestAttribute>(TestEnum.C).Tag);
            Assert.Equal("D", EnumHelper.GetCustomAttribute<TestAttribute>(TestEnum.D).Tag);
        }
    }
}
