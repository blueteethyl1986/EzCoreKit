using System;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Extensions;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class StreamExtensionTest {
        [Fact(DisplayName = "Extensions.Stream.ToBytesAndToStream")]
        public void ToBytesAndToStream_Test() {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 10; i++) {
                byte[] buffer = new byte[rand.Next(0, 32)];
                rand.NextBytes(buffer);

                Assert.Equal(buffer.ToStream().ToBytes(), buffer);
            }
        }
    }
}
