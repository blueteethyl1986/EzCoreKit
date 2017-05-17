using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Math;

namespace EzCoreKit.System.Security.Cryptography {
    public class CSRConfigure {
        public AsymmetricCipherKeyPair KeyPair { get; set; }
        public DateTime NotAfter { get; set; }
        public DateTime NotBefore { get; set; }
        public string SignatureAlgorithm { get; set; }
        public BigInteger SN { get; set; } = BigInteger.ProbablePrime(120, new Random());
        
        public Dictionary<DerObjectIdentifier, string> Attributes
            = new Dictionary<DerObjectIdentifier, string>();
        public Dictionary<DerObjectIdentifier, string> Extensions
            = new Dictionary<DerObjectIdentifier, string>();


        public X509Name GetX509Name() {

            var dict = new Dictionary<DerObjectIdentifier, string>();
            foreach(var kp in Attributes) {
                dict.Add(kp.Key, kp.Value);
            }
            foreach(var kp in Extensions) {
                dict.Add(kp.Key, kp.Value);
            }
            return new X509Name(dict.Keys.ToList(), dict);
        }
    }
}
