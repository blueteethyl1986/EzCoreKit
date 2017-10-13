using System;
using System.Collections.Generic;
using EzCoreKit.Reflection;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Reflection {
    public class MemberInfoExtension_Test {
        [Fact(DisplayName = "Reflection.GetMember")]
        public void GetMember_Test() {
            var member1 = new object().GetMember(x => x.ToString());
            var member2 = typeof(object).GetMember("ToString")[0];

            Assert.Equal(member1, member2);
        }
    }
}
