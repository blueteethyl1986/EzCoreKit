using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EzCoreKit.System.Security.Cryptography {
    public static class RSAFactory {
        public static string GenerateSignature(this AsymmetricKeyParameter key, byte[] data, string signatureAlgorithm = "SHA1withRSA") { 
            var signer = SignerUtilities.GetSigner(signatureAlgorithm);
            signer.Init(true, key);
            signer.BlockUpdate(data, 0, data.Length);
            return BitConverter.ToString(signer.GenerateSignature());
        }

        public static AsymmetricCipherKeyPair GenerateRSAKeyPair(int keySize = 4096) {
            var kpgen = new RsaKeyPairGenerator();
            kpgen.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            return kpgen.GenerateKeyPair();
        }

        public static string ConvertKeyToPEM(AsymmetricKeyParameter key) {
            MemoryStream stream = new MemoryStream();
            TextWriter textWriter = new StreamWriter(stream);
            var writer = new PemWriter(textWriter);
            writer.WriteObject(key);
            writer.Writer.Flush();
            
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static object ConvertPEMToKey(string pem) {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(pem));
            TextReader textReader = new StreamReader(stream);
            var reader = new PemReader(textReader);
            return reader.ReadObject();
        }

        public static (byte[] PublicKey, byte[] PrivateKey) GenerateRSAKeys(int keySize = 4096) {
            using (RSA rsa = RSA.Create()) {
                rsa.KeySize = keySize;
                
                var RSAPrivateParams = rsa.ExportParameters(true);
                var RSAPublicParams = rsa.ExportParameters(false);
                
                return (PublicKey: ExportPublicKey(RSAPublicParams),
                        PrivateKey: ExportPrivateKey(RSAPrivateParams));
            }
        }
        
        private static byte[] ExportPrivateKey(RSAParameters RSAParams) {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            
            writer.Write((byte)0x30); // SEQUENCE
            using (var innerStream = new MemoryStream()) {
                var innerWriter = new BinaryWriter(innerStream);
                EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                EncodeIntegerBigEndian(innerWriter, RSAParams.Modulus);
                EncodeIntegerBigEndian(innerWriter, RSAParams.Exponent);
                if (RSAParams.D != null) {
                    EncodeIntegerBigEndian(innerWriter, RSAParams.D);
                    EncodeIntegerBigEndian(innerWriter, RSAParams.P);
                    EncodeIntegerBigEndian(innerWriter, RSAParams.Q);
                    EncodeIntegerBigEndian(innerWriter, RSAParams.DP);
                    EncodeIntegerBigEndian(innerWriter, RSAParams.DQ);
                    EncodeIntegerBigEndian(innerWriter, RSAParams.InverseQ);
                }
                var length = (int)innerStream.Length;
                EncodeLength(writer, length);
                writer.Write(innerStream.ToArray(), 0, length);
            }

            return stream.ToArray();
        }

        private static byte[] ExportPublicKey(RSAParameters RSAParams) {
            using (var stream = new MemoryStream()) {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream()) {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream()) {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream()) {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, RSAParams.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, RSAParams.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.ToArray(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.ToArray(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.ToArray(), 0, length);
                }

                return stream.ToArray();
            }
        }

        public static (string PublicKey, string PrivateKey) GenerateRSAKeysPEM(int keySize = 4096) {
            var result = GenerateRSAKeys(keySize);

            List<string> PublicKeyList = new List<string>();
            string PublicKeyString = Convert.ToBase64String(result.PublicKey);
            PublicKeyList.Add("-----BEGIN PUBLIC KEY-----");
            for (int i = 0; i < PublicKeyString.Length; i += 64) {
                PublicKeyList.Add(PublicKeyString.Substring(i, Math.Min(64, PublicKeyString.Length - i)));
            }
            PublicKeyList.Add("-----END PUBLIC KEY-----");


            List<string> PrivateKeyList = new List<string>();
            string PrivateKeyString = Convert.ToBase64String(result.PrivateKey);
            PrivateKeyList.Add("-----BEGIN RSA PRIVATE KEY-----");
            for (int i = 0; i < PrivateKeyString.Length; i += 64) {
                PrivateKeyList.Add(PrivateKeyString.Substring(i, Math.Min(64, PrivateKeyString.Length - i)));
            }
            PrivateKeyList.Add("-----END RSA PRIVATE KEY-----");

            return (PublicKey: string.Join("\r\n", PublicKeyList),
                    PrivateKey: string.Join("\r\n", PrivateKeyList));
        }

        private static void EncodeLength(BinaryWriter stream, int length) {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80) {
                // Short form
                stream.Write((byte)length);
            } else {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0) {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--) {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true) {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++) {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0) {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            } else {
                if (forceUnsigned && value[prefixZeros] > 0x7f) {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                } else {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++) {
                    stream.Write(value[i]);
                }
            }
        }
    }
}
