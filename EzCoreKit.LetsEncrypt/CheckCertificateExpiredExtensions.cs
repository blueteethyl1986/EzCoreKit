using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EzCoreKit.LetsEncrypt {
    /// <summary>
    /// 針對Kestrel伺服器提供的Let's Encrypt Https自動檢驗過期擴充方法
    /// </summary>
    public static class CheckCertificateExpiredExtensions {
        /// <summary>
        /// 設定定期檢查Let's Encrypt憑證過期與欲執行<see cref="Action"/>
        /// </summary>
        /// <param name="options">Kestrel伺服器監聽選項實例</param>
        /// <param name="savePassword">憑證儲存密碼</param>
        /// <param name="frequency">檢查頻率</param>
        /// <param name="action">執行方法</param>
        /// <returns>可等候程序</returns>
        public static Task CheckCertificateExpired(this ListenOptions options, string savePassword, TimeSpan frequency, Action action) {
            if (!File.Exists(KestrelLetsEncryptExtension.CertificatePath)) throw new FileNotFoundException(KestrelLetsEncryptExtension.CertificatePath);

            return Task.Run(async () => {
                while (true) {
                    var cert = new X509Certificate2(KestrelLetsEncryptExtension.CertificatePath, savePassword);
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
