using Microsoft.AspNetCore.Server.Kestrel;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EzCoreKit.LetsEncrypt {
    public static partial class KestrelLetsEncryptExtensions {
        /// <summary>
        /// 當憑證即將或已過期時回調指定方法
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="frequency">檢查頻率</param>
        /// <param name="action">回調方法</param>
        public static Task CheckCertificateExpired(this KestrelServerOptions options, string savePassword,TimeSpan frequency, Action action) {
            if (!File.Exists(PfxFilePath)) throw new FileNotFoundException(PfxFilePath);
            
            return Task.Run(async() => {
                while (true) {
                    var cert = new X509Certificate2(PfxFilePath, savePassword);
                    if (cert.NotAfter.ToUniversalTime() + frequency > DateTime.UtcNow) {
                        Console.WriteLine("Certificate Not yet expired");
                        cert.Dispose();       
                    } else {
                        Console.WriteLine("Certificate is about to expire");
                        cert.Dispose();                        
                        action();
                        break;
                    }
                    await Task.Delay(frequency);
                }
            });
        }
    }
}
