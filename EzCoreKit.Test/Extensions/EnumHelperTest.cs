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

        [Fact(DisplayName = "Extensions.Enum.Parse")]
        public void Parse_Test() {
            Assert.Equal(TestEnum.A, EnumHelper.Parse<TestEnum>("A"));
            Assert.Equal(TestEnum.B, EnumHelper.Parse<TestEnum>("B"));
            Assert.Equal(TestEnum.C, EnumHelper.Parse<TestEnum>("C"));
            Assert.Equal(TestEnum.D, EnumHelper.Parse<TestEnum>("D"));
        }

        [Fact(DisplayName = "Extensions.Enum.GetEnumName_Extensions")]
        public void GetEnumNameExtension_Test() {
            Assert.Equal("A", TestEnum.A.GetEnumName());
            Assert.Equal("B", TestEnum.B.GetEnumName());
            Assert.Equal("C", TestEnum.C.GetEnumName());
            Assert.Equal("D", TestEnum.D.GetEnumName());
        }
    }
}
