using System;
using System.IO;
using System.Linq;

namespace EzCoreKit.LetsEncrypt.Tools {
    class Program {
        static void Main(string[] args) {
            var options = GetOptions(args);
            byte[] pfxBinary = LetsEncrypt.KestrelLetsEncryptExtensions.CreateX509BinaryByLetsEncrypt(
                null, options.OutputFilePassword, options.Email, options.Domains.ToArray()); 

            BinaryWriter output = new BinaryWriter(new FileStream(options.OutputFile, FileMode.Create));
            output.Write(pfxBinary);
            output.Flush();
            output.BaseStream.Dispose();
            output.Dispose();
        }

        static Options GetOptions(string[] args) {
            try {
                return (Options)((dynamic)CommandLine.Parser.Default.ParseArguments<Options>(args)).Value;
            } catch {
                return null;
            }
        }
    }
}