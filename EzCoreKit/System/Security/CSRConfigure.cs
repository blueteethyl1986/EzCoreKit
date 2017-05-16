using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Math;

namespace EzCoreKit.System.Security {
    public class CSRConfigure {
        public AsymmetricCipherKeyPair KeyPair { get; set; }
        public DateTime NotAfter { get; set; }
        public DateTime NotBefore { get; set; }
        public string SignatureAlgorithm { get; set; }
        public BigInteger SN { get; set; } = BigInteger.ProbablePrime(120, new Random());

        public Dictionary<DerObjectIdentifier, string> Attributes
            = new Dictionary<DerObjectIdentifier, string>();

        public X509Name GetX509Name() {
            return new X509Name(Attributes.Keys.ToList(), Attributes);
        }
    }
}
