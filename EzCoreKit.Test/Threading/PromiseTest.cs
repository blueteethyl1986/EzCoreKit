using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Threading;
using Xunit;

namespace EzCoreKit.Test.Threading {
    public class PromiseTest {
        [Fact]
        public void Promise_Test() {
            var t = new Promise<int>((res, rej) => {
                rej(new Exception());
            }).Then(x=>100).Catch(x=>-1).Then(x=>x*3).Task.GetAwaiter().GetResult();
            
            Assert.Equal(t, -3);

            var t2 = new Promise<int>((res, rej) => {
                res(100);
            }).Catch(x => -1).Then(x => x * 3).Task.GetAwaiter().GetResult();

            Assert.Equal(t2, 300);
        }
    }
}
