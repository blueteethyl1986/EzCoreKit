using Certes;
using Certes.Acme;
using Certes.Pkcs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.LetsEncrypt {
    public static partial class KestrelLetsEncryptExtensions {
        /// <summary>
        /// 憑證儲存路徑
        /// </summary>
        public static string PfxFilePath { get; set; } = "./letsEncrypt.pfx";

        /// <summary>
        /// 使用Let's Encrypt服務，並設定為Kestrel使用的憑證
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">其他域名</param>
        [Obsolete("請使用UseLetsEncryptAndSave方法")]
        public static void UseLetsEncrypt(this KestrelServerOptions options, string email, params string[] domains) {
            options.UseLetsEncryptAsync(email, domains).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 使用Let's Encrypt服務，並設定為Kestrel使用的憑證(可等候)
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">其他域名</param>
        [Obsolete("請使用UseLetsEncryptAndSaveAsync方法")]
        public static async Task UseLetsEncryptAsync(this KestrelServerOptions options, string email, params string[] domains) {
            await options.UseLetsEncryptAsyncBase(null, email, domains);
        }

        /// <summary>
        /// 優先使用現有憑證，如憑證過期則使用Let's Encrypt服務，並設定為Kestrel使用的憑證與儲存憑證
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="mainDomain">主要域名</param>
        /// <param name="domains">其他域名</param>
        public static void UseLetsEncryptAndSave(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            options.UseLetsEncryptAndSaveAsync(savePassword, email, domains).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 優先使用現有憑證，如憑證過期則使用Let's Encrypt服務，並設定為Kestrel使用的憑證與儲存憑證(可等候)
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="mainDomain">主要域名</param>
        /// <param name="domains">其他域名</param>
        public static async Task UseLetsEncryptAndSaveAsync(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            await options.UseLetsEncryptAsyncBase(savePassword, email, domains);
        }

        private static async Task UseLetsEncryptAsyncBase(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            if (File.Exists(PfxFilePath)) {
                var cert = new X509Certificate2(PfxFilePath, savePassword);

                if (cert.NotAfter > DateTime.UtcNow) {
                    options.UseHttps(PfxFilePath, savePassword);
                    cert.Dispose();
                    return;
                } else {
                    cert.Dispose();
                }
            }
            var pfxBinary = await options.CreateX509BinaryByLetsEncryptAsync(savePassword, email, domains);
            if (savePassword == null) {
                options.UseHttps(new X509Certificate2(pfxBinary));
            } else {
                File.WriteAllBytes(PfxFilePath, pfxBinary);
                options.UseHttps(PfxFilePath, savePassword);
            }
        }

        /// <summary>
        /// 使用Let's Encrypt服務產生X509憑證
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">域名</param>
        /// <returns>X509憑證</returns>
        public static X509Certificate2 CreateX509ByLetsEncrypt(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            return options.CreateX509ByLetsEncryptAsync(savePassword, email, domains).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 使用Let's Encrypt服務產生X509憑證(可等候)
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">域名</param>
        /// <returns>X509憑證</returns>
        public static async Task<X509Certificate2> CreateX509ByLetsEncryptAsync(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            return new X509Certificate2(await options.CreateX509BinaryByLetsEncryptAsync(savePassword, email, domains), savePassword);
        }

        /// <summary>
        /// 使用Let's Encrypt服務產生X509憑證
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">域名</param>
        /// <returns>X509憑證Binary</returns>
        public static byte[] CreateX509BinaryByLetsEncrypt(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            return options.CreateX509BinaryByLetsEncryptAsync(savePassword, email, domains).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 使用Let's Encrypt服務產生X509憑證(可等候)
        /// </summary>
        /// <param name="options">選項</param>
        /// <param name="savePassword">憑證密碼</param>
        /// <param name="email">電子郵件</param>
        /// <param name="domains">域名</param>
        /// <returns>X509憑證Binary</returns>
        public static async Task<byte[]> CreateX509BinaryByLetsEncryptAsync(this KestrelServerOptions options, string savePassword, string email, params string[] domains) {
            using (var client = new AcmeClient(WellKnownServers.LetsEncrypt)) {
                // Create new registration
                var account = await client.NewRegistraton("mailto:" + email);

                // Accept terms of services
                account.Data.Agreement = account.GetTermsOfServiceUri();
                account = await client.UpdateRegistration(account);

                // Initialize authorization
                Dictionary<string, string> keyAuthStringMap = new Dictionary<string, string>();
                List<Challenge> challenges = new List<Challenge>();
                foreach (var domain in domains) {
                    var authz = await client.NewAuthorization(new AuthorizationIdentifier {
                        Type = AuthorizationIdentifierTypes.Dns,
                        Value = domain
                    });

                    // Comptue key authorization for http-01
                    var httpChallengeInfo = authz.Data.Challenges.Where(c => c.Type == ChallengeTypes.Http01).First();
                    challenges.Add(httpChallengeInfo);
                    var keyAuthString = client.ComputeKeyAuthorization(httpChallengeInfo);
                    keyAuthStringMap[httpChallengeInfo.Token] = keyAuthString;
                }

                IWebHost k = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*")
                    .ConfigureServices(services => {
                        services.AddRouting();
                    })
                    .Configure(app => {
                        var trackPackageRouteHandler = new RouteHandler(context => {
                            return context.Response.WriteAsync($"404 Not Found");
                        });

                        var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);
                        routeBuilder.MapGet(".well-known/acme-challenge/{id}", context => {
                            var result = keyAuthStringMap[context.GetRouteValue("id") as string];
                            Console.WriteLine("Server Call :" + context.GetRouteValue("id"));
                            return context.Response.WriteAsync(result);
                        });

                        var routes = routeBuilder.Build();
                        app.UseRouter(routes);


                        Console.WriteLine("Server Start");
                    })
                    .Build();
                k.Start();

                // Do something to fullfill the challenge,
                // e.g. upload key auth string to well known path, or make changes to DNS
                await Task.Delay(10000);

                foreach (var challenge in challenges) {
                    // Info ACME server to validate the identifier
                    var httpChallenge = await client.CompleteChallenge(challenge);
                    //httpChallenge.

                    // Check authorization status
                    var authz = await client.GetAuthorization(httpChallenge.Location);
                    while (authz.Data.Status == EntityStatus.Pending) {
                        await Task.Delay(3000);
                        authz = await client.GetAuthorization(httpChallenge.Location);
                    }
                }

                var csr = new CertificationRequestBuilder();
                csr.AddName("CN", domains.First());
                foreach (var domain in domains.Skip(1)) {
                    csr.SubjectAlternativeNames.Add(domain);
                }
                var cert = await client.NewCertificate(csr);
                var pfxBuilder = cert.ToPfx();

                var x509result = pfxBuilder.Build("letsEncrypt", savePassword);
                Console.WriteLine("Let's Encrypt OK");

                var serviceLife = k.Services.GetService<IApplicationLifetime>();

                serviceLife.StopApplication();
                k.Dispose();
                Console.WriteLine("Server Stop");

                return x509result;
            }
        }

    }
}
