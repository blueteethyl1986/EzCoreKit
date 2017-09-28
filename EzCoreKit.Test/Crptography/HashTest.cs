using EzCoreKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace EzCoreKit.Test.Crptography {
    public class HashTest {
        [Fact(DisplayName = "Extensions.Crptography.Hash.MD5")]
        public void MD5_Test() {
            Assert.Equal(HashHelper.ToHashString<MD5>("123", false), "202cb962ac59075b964b07152d234b70");
        }

        [Fact(DisplayName = "Extensions.Crptography.Hash.SHA256")]
        public void SHA256_Test() {
            Assert.Equal(HashHelper.ToHashString<SHA256>("123", false), "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3");
        }
    }
}
