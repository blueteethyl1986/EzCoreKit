using EzCoreKit.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace EzCoreKit.Test.Threading {
    public class TaskHelperTest {
        [Fact(DisplayName = "Threading.LimitedTask")]
        public void LimitedTask_Test() {
            Assert.Equal(TaskHelper.LimitedTask(() => {

            }, 1000).GetAwaiter().GetResult(), true);
            int value = 0;
            Assert.Equal(TaskHelper.LimitedTask(() => {
                Thread.Sleep(2000);
                value = 5;
            }, 1000).GetAwaiter().GetResult(), false);
            Assert.Equal(value, 0);
        }
    }
}
