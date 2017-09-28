using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using EzCoreKit.Extensions;
using Xunit;

namespace EzCoreKit.Test.Extensions {
    public class BinaryFormatterExtensionTest {
        [Fact(DisplayName = "Extensions.BinaryFormatter.SerializeAndDeserialize")]
        public void BinaryFormatter_SerializeAndDeserialize_Test() {
            var elements = new object[] { true, false, 'a', "S", 1, 0.1 };

            BinaryFormatter bf = new BinaryFormatter();
            foreach (var element in elements) {
                Assert.Equal(bf.Deserialize(element.GetType(), bf.Serialize(element)), element);

                var ary = element.BoxingToArray();
                Assert.Equal(bf.Deserialize(ary.GetType(), bf.Serialize(ary)), ary);
            }
        }
    }
}
