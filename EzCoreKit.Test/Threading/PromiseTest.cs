using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EzCoreKit.Threading;
using Xunit;

namespace EzCoreKit.Test.Threading {
    public class PromiseTest {
        [Fact]
        public void Promise_Test() {
            var t = new Promise<int>((res, rej) => {
                rej(new Exception());
            }).Then(x=>100).Catch(x=>-1).Then(x=>x*3).Task.ToSync();
            
            Assert.Equal(t, -3);

            var t2 = new Promise<int>((res, rej) => {
                res(100);
            }).Catch(x => -1).Then(x => x * 3).Task.ToSync();

            Assert.Equal(t2, 300);


            var p = new Promise<int>((res, rej) => {
                Thread.Sleep(100);
                res(100);
            });

            var p2 = new Promise<int>((res, rej) => {
                Thread.Sleep(200);
                res(200);
            });

            var c = Promise<object>.All(new Promise<object>[] { (Promise<object>)p, (Promise<object>)p2 })
                .Task.ToSync();

            var c2 = Promise<object>.Race(new Promise<object>[] { (Promise<object>)p, (Promise<object>)p2 })
                .Task.ToSync();

            Assert.Equal(c2, 100);
        }
    }
}
