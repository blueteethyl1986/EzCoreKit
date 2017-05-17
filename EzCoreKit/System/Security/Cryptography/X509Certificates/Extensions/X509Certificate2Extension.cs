using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EzCoreKit.System.Security.Cryptography.X509Certificates.Extensions {
    public static class X509Certificate2Extension {
        public static string ToPEM(this X509Certificate2 cert,X509ContentType type= X509ContentType.Pfx) {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            var output = Convert.ToBase64String(cert.Export(type));
            for(int i = 0; i < output.Length; i += 64) {
                builder.AppendLine(output.Substring(i, Math.Min(64, output.Length - i)));
            }
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }
    }
}
