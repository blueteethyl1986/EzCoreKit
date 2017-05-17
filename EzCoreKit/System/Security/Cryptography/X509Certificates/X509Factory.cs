using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EzCoreKit.System.Security.Cryptography.X509Certificates {
    public static class X509Factory {
        public static Pkcs10CertificationRequest GenerateCertificationRequest(CSRConfigure configure) {
            var result = new Pkcs10CertificationRequest(
                configure.SignatureAlgorithm, configure.GetX509Name(),
                configure.KeyPair.Public, null, configure.KeyPair.Private);
            return result;
        }

        public static X509Certificate2 GenerateCertificate(CSRConfigure configure) {
            var gen = new X509V3CertificateGenerator();

            gen.SetPublicKey(configure.KeyPair.Public);
            gen.SetSubjectDN(configure.GetX509Name());
            gen.SetIssuerDN(configure.GetX509Name());
            gen.SetNotAfter(configure.NotAfter);
            gen.SetNotBefore(configure.NotBefore);
            gen.SetSerialNumber(configure.SN);
            gen.SetSignatureAlgorithm(configure.SignatureAlgorithm);

            foreach (var ext in configure.Extensions) {
                gen.AddExtension(ext.Key, false, Encoding.UTF8.GetBytes(ext.Value));
            }
            var result = new X509Certificate2(
                gen.Generate(configure.KeyPair.Private)
                .GetEncoded());

            return result;
        }

        private static AsymmetricKeyParameter ReadKeyParameter(byte[] pemBinary, bool isPublic) {
            if (isPublic) {
                return PublicKeyFactory.CreateKey(pemBinary);
            } else {
                var keyPair = (AsymmetricCipherKeyPair)new Org.BouncyCastle.OpenSsl.PemReader(new StreamReader(new MemoryStream(pemBinary))).ReadObject();
                return keyPair.Private;
            }
        }

    }
}
