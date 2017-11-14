using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EzCoreKit.AspNetCore {
    /// <summary>
    /// 提供簡易Bearer的JWT Toekn實作
    /// </summary>
    public static class EzJwtBearerHelper {
        /// <summary>
        /// 簽名金鑰
        /// </summary>
        public static SecurityKey SigningKey { get; private set; }

        /// <summary>
        /// 簽名演算法
        /// </summary>
        public static string SigningAlgorithm { get; private set; }

        /// <summary>
        /// 發行者
        /// </summary>
        public static string Issuer { get; private set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public static string Audience { get; private set; }

        /// <summary>
        /// 加入簡易JWT Bearer驗證
        /// </summary>
        /// <param name="builder">驗證建構器</param>
        /// <param name="signingKey">簽名金鑰</param>
        /// <param name="signingAlgorithm">簽名演算法</param>
        /// <param name="issuer">發行者</param>
        /// <param name="audience">接收者</param>
        /// <returns>驗證建構器</returns>
        public static AuthenticationBuilder AddEzJwtBearer(
            this AuthenticationBuilder builder,
            SecurityKey signingKey,
            string signingAlgorithm,
            string issuer,
            string audience){
            EzJwtBearerHelper.SigningKey = signingKey;
            EzJwtBearerHelper.SigningAlgorithm = signingAlgorithm ?? SecurityAlgorithms.HmacSha256;
            EzJwtBearerHelper.Issuer = audience ?? issuer;
            EzJwtBearerHelper.Audience = audience;
            
            return builder.AddJwtBearer(o => {
                o.IncludeErrorDetails = true;
                o.SaveToken = true;

                o.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = signingKey,
                    ValidIssuer = issuer,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
            });
        }

        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="expires">過期時間</param>
        /// <param name="claims">權利</param>
        /// <returns>Token字串</returns>
        public static string GenerateToken(DateTime expires, params Claim[] claims) {
            var creds = new SigningCredentials(
                EzJwtBearerHelper.SigningKey, 
                EzJwtBearerHelper.SigningAlgorithm);

            var token = new JwtSecurityToken(
                EzJwtBearerHelper.Issuer,
                EzJwtBearerHelper.Audience,
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
